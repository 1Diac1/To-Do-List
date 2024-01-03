using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using To_Do_List.Application.Common.Filters;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Application.Common.Validators;
using To_Do_List.Application.DTOs;
using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Repositories;
using To_Do_List.Application.Services;

namespace To_Do_List.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        services.AddScoped(typeof(ITodoItemService), typeof(TodoItemService));
        
        services.AddSingleton<IValidationService, ValidationService>();
        services.AddScoped<JwtService>();
        
        services.AddScoped<AuthorizationFilter>();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}