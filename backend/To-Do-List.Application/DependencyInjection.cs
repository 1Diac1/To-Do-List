﻿using Microsoft.Extensions.DependencyInjection;
using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Repositories;
using To_Do_List.Application.Services;

namespace To_Do_List.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        services.AddScoped(typeof(ITodoItemService), typeof(TodoItemService));

        return services;
    }
}