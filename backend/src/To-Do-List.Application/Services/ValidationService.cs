using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using To_Do_List.Application.Interfaces;

namespace To_Do_List.Application.Services;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidator<T> GetValidatorForType<T>()
    {
        var scopedProvider = _serviceProvider.CreateScope().ServiceProvider;
        var service = scopedProvider.GetRequiredService<IValidator<T>>();

        return service;
    }
}