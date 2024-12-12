namespace HealthNoter.DataAccess.Postgresql.Repositories.Base;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
    IEnumerable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    TEntity? GetById(Guid id, CancellationToken ct);
    Task<TEntity?> AddAsync(TEntity entity, CancellationToken ct);
    TEntity? Add(TEntity entity);
    TEntity? Update(TEntity entity);
    TEntity? Delete(TEntity entity);
}