using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using To_Do_List.Application;
using To_Do_List.Application.Common.Filters;
using To_Do_List.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();


builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
    options.Filters.Add<ValidatorActionFilter>();
    options.ModelValidatorProviders.Clear();
})
    .AddNewtonsoftJson();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.MapControllers();

app.Run();
