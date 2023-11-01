using FluentValidation;
using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.Common.Validators;

public class TodoItemValidator : AbstractValidator<TodoItem>
{
    public TodoItemValidator()
    {
        RuleFor(item => item.Title)
            .NotEmpty().WithMessage("Title can't be null")
            .MaximumLength(100).WithMessage("{PropertyName} can't exceed 100 characters");
        
        RuleFor(item => item.Description)
            .NotEmpty().WithMessage("{PropertyName} can't be null")
            .MaximumLength(500).WithMessage("{PropertyName} can't exceed 500 characters");
        
        RuleFor(item => item.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("{PropertyName} should be in the future");
        
        RuleFor(item => item.CompletionDate)
            .Must(BeValidCompletionDate).WithMessage("{PropertyName} must be empty or in the past");

        RuleFor(item => item.Type)
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

    private bool ContainValidTags(IList<TodoTag>? tags) {
        return tags is not null && tags.Count > 0;
    }
}
