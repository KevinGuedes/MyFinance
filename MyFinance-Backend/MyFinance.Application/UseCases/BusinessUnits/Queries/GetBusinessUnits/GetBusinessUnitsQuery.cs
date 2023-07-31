using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

public sealed record GetBusinessUnitsQuery(int Page, int PageSize) : IQuery<IEnumerable<BusinessUnit>>
{
}
