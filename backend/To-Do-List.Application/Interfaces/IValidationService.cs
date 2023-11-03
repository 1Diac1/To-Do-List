using FluentValidation;

namespace To_Do_List.Application.Interfaces;

public interface IValidationService
{
    IValidator<T> GetValidatorForType<T>();
}