using HealthNoter.DataAccess.Postgresql.Repositories;
using HealthNoter.DataAccess.Postgresql.Repositories.Base;

namespace HealthNoter.DataAccess.Postgresql.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IUserRepository? _userRepository;
    private IPressureNoteRepository? _pressureNoteRepository;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository
    {
        get
        {
            _userRepository ??= new UserRepository(_context);
            return _userRepository;
        }
    }
    public IPressureNoteRepository PressureNoteRepository
    {
        get
        {
            _pressureNoteRepository ??= new PressureNoteRepository(_context);
            return _pressureNoteRepository;
        }
    }

    public void Dispose() => _context.Dispose();
    public async Task<int> SaveChangesAsync(CancellationToken ct) => await _context.SaveChangesAsync(ct);
    public async Task BeginTransactionAsync(CancellationToken ct) => await _context.Database.BeginTransactionAsync(ct);
    public async Task CommitTransactionAsync(CancellationToken ct) => await _context.Database.CommitTransactionAsync(ct);
    public async Task RollbackTransactionAsync(CancellationToken ct) => await _context.Database.RollbackTransactionAsync(ct);
}