using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Common;

namespace To_Do_List.Application.Repositories;

// TODO: Решить по поводу AsNoTracking(). Сделать возможность добавления этой функции через параметры
public class EntityRepository<TEntity> : IEntityRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly IApplicationDbContext _dbContext;

    protected EntityRepository(IApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync() =>
        await _dbContext.Set<TEntity>().ToListAsync();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true) =>
        await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();

    public async Task<TEntity> GetByIdAsync(Guid id) =>
        await _dbContext.Set<TEntity>().FindAsync(id);

    public virtual async Task<TEntity> AddAsync(TEntity entity, bool autoSave = true)
    {
        entity.Id = Guid.NewGuid();

        await _dbContext.Set<TEntity>().AddAsync(entity);

        if (autoSave is true)
            await SaveAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true)
    {
        _dbContext.Set<TEntity>().Entry(entity).State = EntityState.Modified;

        if (autoSave is true)
            await SaveAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = true)
    {
        _dbContext.Set<TEntity>().Remove(entity);

        if (autoSave)
            await SaveAsync();
    }

    public async Task SaveAsync() =>
        await _dbContext.SaveChangesAsync();
}