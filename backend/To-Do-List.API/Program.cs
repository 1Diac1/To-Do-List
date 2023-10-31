using To_Do_List.Application;
using To_Do_List.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();