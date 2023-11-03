using System.Text.Json.Serialization;
using To_Do_List.Domain.Common;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.DTOs;

public class TodoItemDTO : BaseEntityDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public TodoStatusTask StatusTask { get; set; }
    public TodoPriorityLevel PriorityLevel { get; set; }

    public IList<TodoTagDTO> Tags { get; set; } = new List<TodoTagDTO>();
}