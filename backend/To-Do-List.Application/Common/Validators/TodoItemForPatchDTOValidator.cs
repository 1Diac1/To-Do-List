using FluentValidation;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.Common.Validators;

public class TodoItemForPatchDTOValidator : AbstractValidator<TodoItemForPatchDTO>
{
    public TodoItemForPatchDTOValidator()
    {
        RuleFor(item => item.StatusTask)
            .IsInEnum().WithMessage("Invalid value for the {PropertyName} type");

        RuleFor(item => item.PriorityLevel)
            .IsInEnum().WithMessage("Invalid value for the {PropertyName} type");
    }
}