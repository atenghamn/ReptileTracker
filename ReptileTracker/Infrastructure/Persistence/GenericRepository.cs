using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReptileTracker.EntityFramework;

namespace ReptileTracker.Infrastructure.Persistence;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    internal ReptileContext _context;
    internal DbSet<TEntity> _dbSet;

    public GenericRepository(ReptileContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _dbSet.ToListAsync(ct);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.ToList();
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _dbSet.FindAsync(new object?[] { id, ct }, ct);
    }

    public TEntity? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public async Task<TEntity?> AddAsync(TEntity entity, CancellationToken ct)
    {
        await _dbSet.AddAsync(entity, ct);
        return entity;
    }

    public TEntity? Add(TEntity entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }


    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }


    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task<int> SaveAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}