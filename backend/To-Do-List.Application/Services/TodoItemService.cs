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

    public async Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName)
    {
        return await base
            .GetAllAsync(item => item.Tags.Any(tag => tag.Name == tagName));
    }

    public async Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true)
    {
        entity.Type = TodoStatusTask.InProgress;
        entity.UserId = user.Id.ToString();
        entity.Created = DateTime.Now;
        entity.Modified = DateTime.Now;

        return await base.AddAsync(entity, autoSave);
    }

    public async Task UpdateAsync(TodoItem entity, bool autoSave = true)
    {
        entity.Modified = DateTime.Now;

        await base.UpdateAsync(entity, autoSave);
    }
}