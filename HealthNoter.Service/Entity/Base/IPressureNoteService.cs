using HealthNoter.Service.DTOs;

namespace HealthNoter.Service.Entity.Base;

public interface IPressureNoteService
{
    Task<bool> Delete(Guid id, CancellationToken ct);
    Task<IEnumerable<PressureNoteDto>> GetAll(CancellationToken ct);
    Task<PressureNoteDto> GetById(Guid id, CancellationToken ct);
    Task<bool> Update(Guid id, int? sys, int? dia, int? pulse, CancellationToken ct);
    Task<bool> Add(int sys, int dia, int pulse, Guid userid, CancellationToken ct);
    Task<IEnumerable<PressureNoteDto>> GetAllByUserId(Guid userid, CancellationToken ct);
}