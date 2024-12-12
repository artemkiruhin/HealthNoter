using HealthNoter.Core.Entities;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HealthNoter.DataAccess.Postgresql.Repositories;

public class PressureNoteRepository : RepositoryBase<PressureNoteEntity>, IPressureNoteRepository
{
    public PressureNoteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PressureNoteEntity>> GetByUserId(Guid userid, CancellationToken ct)
    {
        return await _dbSet.Where(x => x.UserId == userid).ToListAsync(ct);
    }

    public async Task<PressureNoteEntity?> GetByUserIdAndOwnId(Guid userid, Guid id, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.UserId == userid && x.Id == id, ct);
    }
}