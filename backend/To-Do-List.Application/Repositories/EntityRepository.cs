using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Common;

namespace To_Do_List.Application.Repositories;

// TODO: Решить по поводу AsNoTracking(). Сделать возможность добавления этой функции через параметры
public class EntityRepository<TEntity> : IEntityRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly IApplicationDbContext DbContext;

    protected EntityRepository(IApplicationDbContext dbContext)
    {
        this.DbContext = dbContext;
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync() =>
        await DbContext.Set<TEntity>().ToListAsync();

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
        bool disableTracking = true)
    {
        if (disableTracking)
            return await DbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        
        return await DbContext
            .Set<TEntity>()
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true) =>
        await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);

    public async Task<TEntity> GetByIdAsync(Guid id) =>
        await DbContext.Set<TEntity>().FindAsync(id);

    public virtual async Task<TEntity> AddAsync(TEntity entity, bool autoSave = true)
    {
        entity.Id = Guid.NewGuid();

        await DbContext.Set<TEntity>().AddAsync(entity);

        if (autoSave is true)
            await SaveAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true)
    {
        DbContext.Set<TEntity>().Entry(entity).State = EntityState.Modified;

        if (autoSave is true)
            await SaveAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = true)
    {
        DbContext.Set<TEntity>().Remove(entity);

        if (autoSave)
            await SaveAsync();
    }

    public async Task SaveAsync() =>
        await DbContext.SaveChangesAsync();
}