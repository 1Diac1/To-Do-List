using AutoMapper;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.Mappings;

public class TodoItemMapping : Profile
{
    public TodoItemMapping()
    {
        CreateMap<TodoItem, TodoItemDTO>();
        CreateMap<TodoItemDTO, TodoItem>();

        CreateMap<TodoItem, TodoItemForPatchDTO>();
        CreateMap<TodoItemForPatchDTO, TodoItem>();

        CreateMap<TodoItem, TodoItemForUpdatePriorityLevelDTO>();
        CreateMap<TodoItemForUpdatePriorityLevelDTO, TodoItem>();
    }
}