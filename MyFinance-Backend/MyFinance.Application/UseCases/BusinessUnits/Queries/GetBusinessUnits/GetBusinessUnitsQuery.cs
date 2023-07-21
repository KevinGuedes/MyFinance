using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

public sealed class GetBusinessUnitsQuery : IQuery<IEnumerable<BusinessUnit>>
{
}
