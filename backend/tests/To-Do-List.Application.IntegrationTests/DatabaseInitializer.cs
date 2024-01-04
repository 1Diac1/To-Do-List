using To_Do_List.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;
using System.Drawing;

namespace To_Do_List.Application.IntegrationTests;

public static class DatabaseInitializer
{
    public static async Task<DbContextOptions<ApplicationDbContext>> InitializeAsync(string databaseName)
    {
        DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        await using var context = new ApplicationDbContext(dbContextOptions);

        await context.TodoItems.AddAsync(new TodoItem()
        {
            Id = Guid.Parse("620ba1b1-d8a3-4570-a0a4-88dbfaa28201"),
            Created = DateTime.Now,
            Modified = DateTime.Now,
            Title = "Something title",
            Description = "Something description",
            PriorityLevel = TodoPriorityLevel.Low,
            StatusTask = TodoStatusTask.Completed,
            UserId = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8"),
            Tags = new List<TodoTag>()
            {
                new TodoTag()
                {
                    Id = Guid.Parse("7edcd1b0-0ae9-486b-ae5d-f44e9a091df0"),
                    Name = "Something name",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    UserId = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8"),
                    Color = KnownColor.Aqua,
                    TodoItemId = Guid.Parse("620ba1b1-d8a3-4570-a0a4-88dbfaa28201")
                }
            }
        });

        await context.SaveChangesAsync();

        return dbContextOptions;
    }
}