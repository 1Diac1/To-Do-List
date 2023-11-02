using System.Text.Json.Serialization;

namespace To_Do_List.Domain.Entities;

public class TodoItem : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public TodoStatusTask Type { get; set; }
    public TodoPriorityLevel TodoPriorityLevel { get; set; }

    public IList<TodoTag> Tags { get; set; } = new List<TodoTag>();
}