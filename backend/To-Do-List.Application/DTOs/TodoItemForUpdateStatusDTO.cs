using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.DTOs;

public class TodoItemForUpdateStatusDTO
{
    public TodoStatusTask Type { get; set; }
}