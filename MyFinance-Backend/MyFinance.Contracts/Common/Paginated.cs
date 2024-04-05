namespace MyFinance.Contracts.Common;

public class Paginated<T>(IReadOnlyCollection<T> items, int pageNumber, int pageSize, long totalCount)
{
    public IReadOnlyCollection<T> Items { get; init; } = items;
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public long TotalCount { get; init; } = totalCount;
}