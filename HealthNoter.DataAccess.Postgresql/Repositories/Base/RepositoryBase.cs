using Microsoft.EntityFrameworkCore;

namespace HealthNoter.DataAccess.Postgresql.Repositories.Base;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.ToList();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FindAsync(id, ct);
    }

    public TEntity? GetById(Guid id, CancellationToken ct)
    {
        return _dbSet.Find(id, ct);
    }

    public async Task<TEntity?> AddAsync(TEntity entity, CancellationToken ct)
    {
        var entry = await _dbSet.AddAsync(entity, ct);
        return entry.Entity;
    }

    public TEntity? Add(TEntity entity)
    {
        var entry = _dbSet.Add(entity);
        return entry.Entity;
    }

    public TEntity? Update(TEntity entity)
    {
        var entry = _dbSet.Update(entity);
        return entry.Entity;
    }

    public TEntity? Delete(TEntity entity)
    {
        var entry = _dbSet.Remove(entity);
        return entry.Entity;
    }
}