namespace TransactEase.Models;

public class FilterCondition
{
    public required string Field { get; set; }
    public required string Operator { get; set; }
    public required dynamic Value { get; set; }
}

public class FilterPayload
{
    public string Logic { get; set; } = "or"; // "and" | "or"
    public required List<FilterCondition> Conditions { get; set; }
}

public class Paging
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchRequest
{
    public FilterPayload? Filter { get; set; }
    public Paging Pagination { get; set; } = new();
}

public class PaginatedResponse<T>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public List<T>? Data { get; set; }
}