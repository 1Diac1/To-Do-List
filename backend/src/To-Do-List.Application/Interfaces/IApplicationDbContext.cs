using Microsoft.EntityFrameworkCore;
using To_Do_List.Domain.Common;
using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

    DbSet<TodoItem> TodoItems { get; set; }
    DbSet<TodoTag> TodoTags { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}