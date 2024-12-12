using HealthNoter.Core.Entities;
using HealthNoter.DataAccess.Postgresql.UoW;
using HealthNoter.Service.DTOs;
using HealthNoter.Service.Entity.Base;

namespace HealthNoter.Service.Entity;

public class PressureNoteService : IPressureNoteService
{
    private readonly IUnitOfWork _database;

    public PressureNoteService(IUnitOfWork database)
    {
        _database = database;
    }
    
    public async Task<bool> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _database.BeginTransactionAsync(ct);

            var note = await _database.PressureNoteRepository.GetByIdAsync(id, ct);
            if (note == null) throw new KeyNotFoundException("Запись не найдена");

            var result = _database.PressureNoteRepository.Delete(note) != null;
            
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

    public async Task<IEnumerable<PressureNoteDto>> GetAll(CancellationToken ct)
    {
        var notes = await _database.PressureNoteRepository.GetAllAsync(ct);
        var dtos = notes.Select(x => 
            new PressureNoteDto(x.Id, x.Sys, x.Dia, x.Pulse, x.CreatedAt, x.UserId, x.User.Username));
        return dtos;
    }

    public async Task<PressureNoteDto> GetById(Guid id, CancellationToken ct)
    {
        var note = await _database.PressureNoteRepository.GetByIdAsync(id, ct);
        var dto = new PressureNoteDto(note.Id, note.Sys, note.Dia, note.Pulse, note.CreatedAt, note.UserId, note.User.Username);
        return dto;
    }

    public async Task<bool> Update(Guid id, int? sys, int? dia, int? pulse, CancellationToken ct)
    {
        try
        {
            if (sys == null && dia == null && pulse == null)
                throw new ArgumentException("Все параметры не могут быть пустыми");
            
            await _database.BeginTransactionAsync(ct);

            var note = await _database.PressureNoteRepository.GetByIdAsync(id, ct);
            if (note == null) throw new KeyNotFoundException("Запись не найдена");

            if (sys != null) note.Sys = sys.Value;
            if (dia != null) note.Dia = dia.Value;
            if (pulse != null) note.Pulse = pulse.Value;

            var result = _database.PressureNoteRepository.Update(note) != null;
            
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

    public async Task<bool> Add(int sys, int dia, int pulse, Guid userid, CancellationToken ct)
    {
        try
        {
            await _database.BeginTransactionAsync(ct);

            var note = PressureNoteEntity.Create(sys, dia, pulse, userid);
            
            var result = await _database.PressureNoteRepository.AddAsync(note, ct) != null;
            
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