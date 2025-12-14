namespace TransactEase.Helper;


using Dapper;
using System.Text;
using TransactEase.Enums;
using TransactEase.Models;

public class DynamicQueryBuilder
{
    public static (string Sql, DynamicParameters Parameters) BuildQuery(
        string baseQuery,
        SearchRequest? searchRequest,
        Dictionary<string, FieldMapping> fieldMap)
    {
        (StringBuilder whereClause, DynamicParameters parameters) = BuildWhereClause(searchRequest?.Filter, fieldMap);
        string whereClauseString = whereClause.ToString();

        string pagingQuery = BuildPagingQuery(searchRequest?.Pagination, parameters);

        var finalQuery = $@"
            {baseQuery}
            {whereClauseString}
            ORDER BY 1 
            { pagingQuery }
        ";

        return (finalQuery, parameters);
    }

    public static (StringBuilder whereClause, DynamicParameters parameters) BuildWhereClause(
        FilterPayload? filterPayload,
        Dictionary<string, FieldMapping> fieldMap
    )
    {
        DynamicParameters parameters = new();
        StringBuilder whereClause = new();

        // Build WHERE clause
        if (filterPayload?.Conditions != null && filterPayload.Conditions.Count != 0)
        {
            List<string> conditions = [];

            for (int i = 0; i < filterPayload.Conditions.Count; i++)
            {
                var condition = filterPayload.Conditions[i];

                // Validate field exists in map
                if (!fieldMap.TryGetValue(condition.Field, out FieldMapping? mapping))
                {
                    throw new ArgumentException($"Invalid field: {condition.Field}");
                }

                var paramName = $"param{i}";

                // Build condition based on operator and type
                var sqlCondition = BuildCondition(
                    mapping.SqlColumn,
                    condition.Operator,
                    condition.Value,
                    mapping.Type,
                    paramName,
                    parameters
                );

                conditions.Add(sqlCondition);
            }

            if (conditions.Any())
            {
                var logicOperator = filterPayload.Logic == "or" ? " OR " : " AND ";
                whereClause.Append(" AND ");
                whereClause.Append(string.Join(logicOperator, conditions));
            }
        }

        return (whereClause, parameters);
    }

    public static string BuildPagingQuery(Paging? pagination, DynamicParameters parameters )
    {
        if (pagination == null)
        {
            return "";
        }
        parameters.Add("Limit", pagination.PageSize);
        parameters.Add("Offset", (pagination.Page - 1) * pagination.PageSize);

        return $"LIMIT {pagination.PageSize} OFFSET {(pagination.Page - 1) * pagination.PageSize}";
    }

    public static (string Sql, DynamicParameters Parameters) BuildCountQuery(
        string baseQuery,
        FilterPayload? filter,
        Dictionary<string, FieldMapping> fieldMap)
    {
        (StringBuilder whereClause, DynamicParameters parameters) = BuildWhereClause(filter, fieldMap);
        string whereClauseString = whereClause.ToString();

        var countSql = $@"
            SELECT COUNT(*) 
            FROM ({baseQuery}) AS base_query
            WHERE 1=1 {whereClauseString}
        ";

        return (countSql, parameters);
    }

    public static string BuildCondition(
        string sqlColumn,
        string operatorType,
        object value,
        FieldTypeEnum fieldType,
        string paramName,
        DynamicParameters parameters)
    {
        switch (operatorType.ToLower())
        {
            case "equals":
                parameters.Add(paramName, value);
                return $"{sqlColumn} = @{paramName}";

            case "notequals":
                parameters.Add(paramName, value);
                return $"{sqlColumn} != @{paramName}";

            case "contains":
                if (fieldType != FieldTypeEnum.String && fieldType != FieldTypeEnum.Date && fieldType != FieldTypeEnum.Number)
                    throw new ArgumentException("Contains operator only works with string, date or number");
                if (fieldType == FieldTypeEnum.Date)
                {
                    parameters.Add(paramName, $"%{value}%");
                    return $"to_char({sqlColumn}, 'YYYY-MM-DD') ILIKE @{paramName}";
                }
                if (fieldType == FieldTypeEnum.Number)
                {
                    parameters.Add(paramName, $"%{value}%");
                    return $"CAST({sqlColumn} AS TEXT) ILIKE @{paramName}";
                }
                parameters.Add(paramName, $"%{value}%");
                return $"{sqlColumn} ILIKE @{paramName}";

            case "startswith":
                if (fieldType != FieldTypeEnum.String && fieldType != FieldTypeEnum.Date && fieldType != FieldTypeEnum.Number)
                    throw new ArgumentException("StartsWith operator only works with string, date or number");
                if (fieldType == FieldTypeEnum.Date)
                {
                    parameters.Add(paramName, $"{value}%");
                    return $"to_char({sqlColumn}, 'YYYY-MM-DD') ILIKE @{paramName}";
                }
                if (fieldType == FieldTypeEnum.Number)
                {
                    parameters.Add(paramName, $"{value}%");
                    return $"CAST({sqlColumn} AS TEXT) ILIKE @{paramName}";
                }
                parameters.Add(paramName, $"{value}%");
                return $"{sqlColumn} ILIKE @{paramName}";

            case "endswith":
                if (fieldType != FieldTypeEnum.String && fieldType != FieldTypeEnum.Date && fieldType != FieldTypeEnum.Number)
                    throw new ArgumentException("EndsWith operator only works with string, date or number");
                if (fieldType == FieldTypeEnum.Date)
                {
                    parameters.Add(paramName, $"%{value}");
                    return $"to_char({sqlColumn}, 'YYYY-MM-DD') ILIKE @{paramName}";
                }
                if (fieldType == FieldTypeEnum.Number)
                {
                    parameters.Add(paramName, $"%{value}");
                    return $"CAST({sqlColumn} AS TEXT) ILIKE @{paramName}";
                }
                parameters.Add(paramName, $"%{value}");
                return $"{sqlColumn} ILIKE @{paramName}";

            case "greaterthan":
                if (fieldType == FieldTypeEnum.String)
                    throw new ArgumentException("GreaterThan doesn't work with string fields");
                parameters.Add(paramName, value);
                return $"{sqlColumn} > @{paramName}";

            case "lessthan":
                if (fieldType == FieldTypeEnum.String)
                    throw new ArgumentException("LessThan doesn't work with string fields");
                parameters.Add(paramName, value);
                return $"{sqlColumn} < @{paramName}";

            case "between":
                if (fieldType == FieldTypeEnum.String)
                    throw new ArgumentException("Between doesn't work with string fields");
                // Expect value to be array: [min, max]
                var values = (value as IEnumerable<object>)?.ToArray();
                if (values == null || values.Length != 2)
                    throw new ArgumentException("Between operator requires array of 2 values");
                parameters.Add($"{paramName}Min", values[0]);
                parameters.Add($"{paramName}Max", values[1]);
                return $"{sqlColumn} BETWEEN @{paramName}Min AND @{paramName}Max";

            case "in":
                // Expect value to be array
                var inValues = (value as IEnumerable<object>)?.ToArray();
                if (inValues == null || !inValues.Any())
                    throw new ArgumentException("In operator requires non-empty array");
                parameters.Add(paramName, inValues);
                return $"{sqlColumn} = ANY(@{paramName})";

            case "isnull":
                return $"{sqlColumn} IS NULL";

            case "isnotnull":
                return $"{sqlColumn} IS NOT NULL";

            default:
                throw new ArgumentException($"Unsupported operator: {operatorType}");
        }
    }
}