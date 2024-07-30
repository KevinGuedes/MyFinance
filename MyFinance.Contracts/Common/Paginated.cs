namespace MyFinance.Contracts.Common;

public sealed class Paginated<T>
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required long TotalCount { get; init; }
    public bool HasNextPage => TotalCount > PageSize * PageNumber;
    public bool HasPreviousPage => PageNumber > 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public required IReadOnlyCollection<T> Items { get; init; }
}