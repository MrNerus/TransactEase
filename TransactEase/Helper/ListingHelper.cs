using Dapper;
using System.Data;
using System.Text;
using TransactEase.Models;

namespace TransactEase.Helper;

public class ListingService
{
    public async static Task<PaginatedResponse<T>> GetPagedListAsync<T>(
        string baseQuery,
        SearchRequest? searchRequest,
        Dictionary<string, FieldMapping> fieldMap,
        IDbConnection conn,
        IDbTransaction? tran = null)
    {
        var (query, parameters) = DynamicQueryBuilder.BuildQuery(
            baseQuery,
            searchRequest,
            fieldMap
        );

        var items = await conn.QueryAsync<T>(query, parameters);

        var countQuery = DynamicQueryBuilder.BuildCountQuery(baseQuery, searchRequest?.Filter, fieldMap);

        var totalCount = await conn.ExecuteScalarAsync<int>(
            countQuery.Sql,
            countQuery.Parameters,
            tran
        );

        return new PaginatedResponse<T>
        {
            Data = [.. items],
            TotalCount = totalCount,
            Page = searchRequest?.Pagination?.Page ?? 1,
            PageSize = searchRequest?.Pagination?.PageSize ?? totalCount,
            TotalPages = searchRequest?.Pagination == null ? 1 : (int)Math.Ceiling(totalCount / (double)searchRequest.Pagination.PageSize)
        };
    }

    
}
