using FluentAssertions;
using To_Do_List.Application.Common.Validators;
using To_Do_List.Application.DTOs;

namespace To_Do_List.Application.UnitTests.Validators;

public class TodoItemDTOValidatorTests
{
    private readonly TodoItemDTOValidator _validator;

    public TodoItemDTOValidatorTests()
    {
        _validator = new TodoItemDTOValidator();
    }

    [Fact] 
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        // arrange
        var todoItemDto = new TodoItemDTO() { Title = string.Empty };
        
        // act
        var result = _validator.Validate(todoItemDto);
        
        // assert
        Assert.False(result.IsValid);
    }
}