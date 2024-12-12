using HealthNoter.Core.Entities;

namespace HealthNoter.DataAccess.Postgresql.Repositories.Base;

public interface IPressureNoteRepository : IRepository<PressureNoteEntity>
{
    Task<IEnumerable<PressureNoteEntity>> GetByUserId(Guid userid, CancellationToken ct);
    Task<PressureNoteEntity?> GetByUserIdAndOwnId(Guid userid, Guid id, CancellationToken ct);
}