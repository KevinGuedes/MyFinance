﻿using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Persistence.Repositories;

public interface IBusinessUnitRepository
{
    Task<IEnumerable<BusinessUnit>> GetPaginatedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetWithSummaryData(Guid id, int year, CancellationToken cancellationToken);
    Task<BusinessUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Update(BusinessUnit businessUnit);
    void Insert(BusinessUnit businessUnit);
}