using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.HealthCheck.Responses;

namespace MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthReport;

public sealed record GetHealthReportQuery : IQuery<HealthReportResponse>
{
}
