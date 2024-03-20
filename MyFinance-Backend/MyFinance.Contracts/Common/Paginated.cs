namespace MyFinance.Contracts.Common;

public class Paginated<T>(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalCount)
{
    public IReadOnlyCollection<T> Items { get; init; } = items;
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public int TotalCount { get; init; } = totalCount;
}