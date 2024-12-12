using HealthNoter.Core.Entities;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HealthNoter.DataAccess.Postgresql.Repositories;

public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByUsernameAndPassword(string username, string password, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Username == username && x.Password == password, ct);
    }

    public async Task<UserEntity?> GetByUsername(string username, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Username == username, ct);
    }
}