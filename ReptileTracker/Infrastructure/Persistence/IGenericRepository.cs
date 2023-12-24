using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReptileTracker.Infrastructure.Persistence;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct);
    IEnumerable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct);
    TEntity? GetById(int id);
    Task<TEntity?> AddAsync(TEntity entity);
    TEntity? Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Save();
    Task<int> SaveAsync(CancellationToken ct);

}