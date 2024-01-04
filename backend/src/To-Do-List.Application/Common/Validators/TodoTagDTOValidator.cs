using System.Drawing;
using FluentValidation;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.Common.Validators;

public class TodoTagDTOValidator : AbstractValidator<TodoTagDTO>
{
    public TodoTagDTOValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty().WithMessage("{PropertyName} can't be null")
            .MaximumLength(100).WithMessage("{PropertyName} can't exceed 100 characters");

        RuleFor(item => item.Color)
            .Must(BeValidColor).WithMessage("{PropertyName} is not allowed");
    }

    private bool BeValidColor(KnownColor color)
    {
        var colorNames = Enum.GetNames(typeof(KnownColor));

        return colorNames.Contains(Enum.GetName(color));
    }
}