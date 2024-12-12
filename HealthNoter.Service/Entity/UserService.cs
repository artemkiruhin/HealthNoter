using HealthNoter.Core.Entities;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;
using HealthNoter.DataAccess.Postgresql.UoW;
using HealthNoter.Service.DTOs;
using HealthNoter.Service.Entity.Base;

namespace HealthNoter.Service.Entity;

public class UserService : IUserService
{
    private readonly IUnitOfWork _database;

    public UserService(IUnitOfWork database)
    {
        _database = database;
    }

    public async Task<bool> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _database.BeginTransactionAsync(ct);

            var user = await _database.UserRepository.GetByIdAsync(id, ct);
            if (user == null) throw new KeyNotFoundException("Пользователь не найден");
            var result = _database.UserRepository.Delete(user) != null;

            await _database.SaveChangesAsync(ct);

            await _database.CommitTransactionAsync(ct);
            return result;
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            throw;
        }

    }

    public async Task<IEnumerable<UserDto>> GetAll(CancellationToken ct)
    {
        var users = await _database.UserRepository.GetAllAsync(ct);
        var dtos = users.Select(x => new UserDto(x.Id, x.Username, x.RegisteredAt));
        
        return dtos;
    }

    public async Task<UserDto> GetById(Guid id, CancellationToken ct)
    {
        var user = await _database.UserRepository.GetByIdAsync(id, ct);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден");
        
        var dto = new UserDto(user.Id, user.Username, user.RegisteredAt);
        
        return dto;
    }

    public async Task<bool> Update(Guid id, string password, CancellationToken ct)
    {
        try
        {
            await _database.BeginTransactionAsync(ct);
            var user = await _database.UserRepository.GetByIdAsync(id, ct);
            if (user == null) throw new KeyNotFoundException("Пользователь не найден");

            user.Password = password;
            
            var result = _database.UserRepository.Update(user) != null;
            
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            return result;
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            throw;
        }
        
    }

    public async Task<bool> Add(string username, string password, CancellationToken ct)
    {
        try
        {
            var user = UserEntity.Create(username, password);

            await _database.BeginTransactionAsync(ct);
            
            var result = await _database.UserRepository.AddAsync(user, ct) != null;
            
            await _database.SaveChangesAsync(ct);
            await _database.CommitTransactionAsync(ct);

            return result;
        }
        catch (Exception e)
        {
            await _database.RollbackTransactionAsync(ct);
            throw;
        }
        
    }
}