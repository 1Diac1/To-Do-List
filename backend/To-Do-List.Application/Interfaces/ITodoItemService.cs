using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Common;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Interfaces;

public interface ITodoItemService : IEntityRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName);
    Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true);
    Task<TodoItem> GetAsync(Guid id, ApplicationUser user);
    Task UpdatePatchAsync<T>(TodoItem entity, JsonPatchDocument entityForUpdate) where T : BaseEntityDTO;
    Task DeleteAsync(Guid id, ApplicationUser user, bool autoSave = true);
}