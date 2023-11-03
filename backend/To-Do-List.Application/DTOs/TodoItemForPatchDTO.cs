using To_Do_List.Domain.Common;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.DTOs;

public class TodoItemForPatchDTO : BaseEntityDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public TodoStatusTask? StatusTask { get; set; }
    public TodoPriorityLevel? PriorityLevel { get; set; }
}