using HealthNoter.Core.Entities;

namespace HealthNoter.DataAccess.Postgresql.Repositories.Base;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByUsernameAndPassword(string username, string password, CancellationToken ct);
    Task<UserEntity?> GetByUsername(string username, CancellationToken ct);
}