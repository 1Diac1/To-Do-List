using System.Linq.Expressions;
using To_Do_List.Domain.Common;

namespace To_Do_List.Application.Interfaces;

public interface IEntityRepository<TEntity> where TEntity : BaseEntity
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true);

    Task<TEntity> GetByIdAsync(Guid id);
    Task<TEntity> AddAsync(TEntity entity, bool autoSave = true);
    Task UpdateAsync(TEntity entity, bool autoSave = true);
    Task DeleteAsync(TEntity entity, bool autoSave = true);
    Task SaveAsync();
}