using To_Do_List.Application.Mappings;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;
using System.Drawing;
using AutoMapper;

namespace To_Do_List.Application.UnitTests.Mappings;

public class MappingTests
{
    [Fact]
    public void Should_Map_Entity_To_Dto_Correctly()
    {
        // arrange
        var todoTag = new TodoTag() { Color = KnownColor.Aqua }; 
        var todoItem = new TodoItem { Tags = new List<TodoTag>() { todoTag } };

        var mappingProfiles = new List<Profile>() { new TodoItemMapping(), new TodoTagMapping() };
        var mapper = new Mapper(new MapperConfiguration(cfg => 
            cfg.AddProfiles(mappingProfiles)));
        
        // act
        var dto = mapper.Map<TodoItemDTO>(todoItem);

        // assert
        Assert.Equal(todoTag.Color, dto.Tags.FirstOrDefault()!.Color);
    }
}