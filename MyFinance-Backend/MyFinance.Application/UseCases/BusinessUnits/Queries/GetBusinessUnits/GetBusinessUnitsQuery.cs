using MyFinance.Application.Common.RequestHandling.Queries;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.UseCases.BusinessUnits.Queries.GetBusinessUnits;

public sealed class GetBusinessUnitsQuery : IQuery<IEnumerable<BusinessUnit>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public GetBusinessUnitsQuery(int page, int pageSize)
        => (Page, PageSize) = (page, pageSize);
}
