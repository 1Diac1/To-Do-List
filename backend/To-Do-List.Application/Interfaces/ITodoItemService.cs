using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.Interfaces;

public interface ITodoItemService : IEntityRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName);
}