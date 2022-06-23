namespace MyFinance.Infra.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken);
    }
}
