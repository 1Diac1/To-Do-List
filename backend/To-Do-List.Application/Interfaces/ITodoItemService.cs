using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Interfaces;

public interface ITodoItemService : IEntityRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName);
    Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true);
    Task DeleteAsync(Guid id, ApplicationUser user, bool autoSave = true);
}