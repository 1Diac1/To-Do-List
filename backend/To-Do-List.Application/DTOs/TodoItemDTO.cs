using System.Text.Json.Serialization;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.DTOs;

public class TodoItemDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public TodoStatusTask Type { get; set; }
    public TodoPriorityLevel TodoPriorityLevel { get; set; }

    public IList<TodoTagDTO> Tags { get; set; } = new List<TodoTagDTO>();
}