namespace MyFinance.Contracts.Common;

public class PaginatedResponse<T>(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
{
    public IReadOnlyCollection<T> Items { get; init; } = items.ToList().AsReadOnly();
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public int TotalCount { get; init; } = totalCount;
}
