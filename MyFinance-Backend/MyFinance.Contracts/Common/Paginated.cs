namespace MyFinance.Contracts.Common;

public sealed class Paginated<T>(IReadOnlyCollection<T> items, int pageNumber, int pageSize, long totalCount)
{
    public IReadOnlyCollection<T> Items { get; init; } = items;
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public long TotalCount { get; init; } = totalCount;
    public bool HasNextPage => TotalCount > PageSize * PageNumber;
    public bool HasPreviousPage => PageNumber > 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}