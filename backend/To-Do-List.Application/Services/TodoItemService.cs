using Microsoft.EntityFrameworkCore;
using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Repositories;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Services;

public class TodoItemService : EntityRepository<TodoItem>, ITodoItemService
{
    public TodoItemService(IApplicationDbContext dbContext) 
        : base(dbContext)
    { }

    public override async Task<IReadOnlyList<TodoItem>> GetAllAsync()
    {
        return await DbContext.TodoItems
            .Include(item => item.Tags)
            .ToListAsync();
    }

    public override async Task<TodoItem> GetByIdAsync(Guid id)
    {
        return await DbContext.TodoItems
            .Include(item => item.Tags)
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName)
    {
        return await base
            .GetAllAsync(item => item.Tags.Any(tag => tag.Name == tagName));
    }

    public async Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true)
    {
        entity.Type = TodoStatusTask.InProgress;
        entity.UserId = user.Id;
        entity.Created = DateTime.Now;
        entity.Modified = DateTime.Now;

        return await base.AddAsync(entity, autoSave);
    }

    public override async Task UpdateAsync(TodoItem entity, bool autoSave = true)
    {
        entity.Modified = DateTime.Now;

        await base.UpdateAsync(entity, autoSave);
    }

    public async Task DeleteAsync(Guid id, ApplicationUser user, bool autoSave = true)
    {
        var todo = await base
            .GetAsync(item => item.Id == id && item.UserId == user.Id);
        
        await base.DeleteAsync(todo, autoSave);
    }
}