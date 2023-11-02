using FluentValidation;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.Common.Validators;

public class TodoItemDTOValidator : AbstractValidator<TodoItemDTO>
{
    public TodoItemDTOValidator()
    {
        RuleFor(item => item.Title)
            .NotEmpty().WithMessage("Title can't be null")
            .MaximumLength(100).WithMessage("{PropertyName} can't exceed 100 characters");
        
        RuleFor(item => item.Description)
            .NotEmpty().WithMessage("{PropertyName} can't be null")
            .MaximumLength(500).WithMessage("{PropertyName} can't exceed 500 characters");
        
        RuleFor(item => item.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("{PropertyName} should be in the future");

        RuleFor(item => item.Type)
            .IsInEnum().WithMessage("Invalid value for the {PropertyName} type");
        
        RuleFor(item => item.TodoPriorityLevel)
            .IsInEnum().WithMessage("Invalid value for the {PropertyName} type");

        RuleFor(item => item.Tags)
            .Must(ContainValidTags).WithMessage("{PropertyName} can't be empty and must contain valid tags");
    }

    private bool BeValidCompletionDate(DateTime? completionDate)
    {
        if (completionDate.HasValue is false)
            return true;

        return completionDate <= DateTime.Now;
    }

    private bool ContainValidTags(IList<TodoTagDTO>? tags) {
        return tags is not null && tags.Count > 0;
    }
}
