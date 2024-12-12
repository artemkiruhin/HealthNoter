using HealthNoter.Service.DTOs;

namespace HealthNoter.Service.Entity.Base;

public interface IUserService
{
    Task<bool> Delete(Guid id, CancellationToken ct);
    Task<IEnumerable<UserDto>> GetAll(CancellationToken ct);
    Task<UserDto> GetById(Guid id, CancellationToken ct);
    Task<bool> Update(Guid id, string password, CancellationToken ct);
    Task<bool> Add(string username, string password, CancellationToken ct);
}