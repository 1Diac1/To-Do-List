using Microsoft.AspNetCore.JsonPatch;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Interfaces;

public interface ITodoItemService : IEntityRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName);
    Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true);
    Task<TodoItem> GetAsync(Guid id, ApplicationUser user);
    Task UpdatePatchAsync(TodoItem entity, JsonPatchDocument<TodoItemForUpdateStatusDTO> document);
    Task DeleteAsync(Guid id, ApplicationUser user, bool autoSave = true);
}