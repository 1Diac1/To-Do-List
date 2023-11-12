using AutoMapper;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.Mappings;

public class TodoTagMapping : Profile
{
    public TodoTagMapping()
    {
        CreateMap<TodoTag, TodoTagDTO>();
        CreateMap<TodoTagDTO, TodoTag>();
    }
}