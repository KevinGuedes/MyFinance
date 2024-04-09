using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyFinance.Contracts.HealthCheck.Responses;

public sealed class InstanceStatusResponse
{
    public required string Name { get; init; }
    public required string Status { get; init; }
    public required string Duration { get; init; }
    public required IReadOnlyDictionary<string, object> Data { get; init; }
    public required IReadOnlyCollection<string> Tags { get; init; }
    public string? Description { get; init; }
}
