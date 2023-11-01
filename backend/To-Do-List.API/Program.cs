using Microsoft.AspNetCore.Mvc;
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
});

var app = builder.Build();

app.MapControllers();

app.Run();