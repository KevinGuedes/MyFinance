using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.HealthCheck.Responses;

namespace MyFinance.Application.UseCases.HealthChecks.Queries.GetHealthChecksReport;

public sealed record GetHealthChecksReportQuery : IQuery<HealthChecksReportResponse>
{
}
