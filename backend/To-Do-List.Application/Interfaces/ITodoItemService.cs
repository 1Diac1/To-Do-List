using Microsoft.AspNetCore.JsonPatch;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Common;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Interfaces;

public interface ITodoItemService : IEntityRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName, ApplicationUser user);
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByStatusTaskAsync(TodoStatusTask status, ApplicationUser user);
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByDueDateAsync(DateTime date, ApplicationUser user);
    Task<IReadOnlyList<TodoItem>> GetSortedIncompleteTodoItemsAsync(ApplicationUser user);
    Task<TodoItemStatistics> GetTodoItemsStatisticsAsync(ApplicationUser user);
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByUserIdAsync(Guid id);
    Task<IReadOnlyList<TodoTag>> GetTodoTagsAsync(ApplicationUser user);
    Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true);
    Task<TodoItem> GetAsync(Guid id, ApplicationUser user);
    Task UpdatePatchAsync<T>(TodoItem entity, JsonPatchDocument<T> entityForUpdate) where T : BaseEntityDTO;
    Task DeleteAsync(Guid id, ApplicationUser user, bool autoSave = true);
}