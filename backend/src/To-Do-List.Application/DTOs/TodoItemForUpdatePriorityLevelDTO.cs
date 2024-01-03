using To_Do_List.Domain.Common;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.DTOs;

public class TodoItemForUpdatePriorityLevelDTO : BaseEntityDTO
{
    public TodoPriorityLevel PriorityLevel { get; set; }
}