using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Common;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;

namespace To_Do_List.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity =>
        base.Set<TEntity>();

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<TodoTag> TodoTags { get; set; }
}