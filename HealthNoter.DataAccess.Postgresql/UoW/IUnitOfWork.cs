using HealthNoter.DataAccess.Postgresql.Repositories.Base;

namespace HealthNoter.DataAccess.Postgresql.UoW;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IPressureNoteRepository PressureNoteRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitTransactionAsync(CancellationToken ct);
    Task RollbackTransactionAsync(CancellationToken ct);
    public void Dispose();
}