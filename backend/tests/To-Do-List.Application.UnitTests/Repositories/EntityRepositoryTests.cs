using System.Drawing;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Repositories;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.UnitTests.Repositories;

public class EntityRepositoryTests
{
    private readonly Mock<IApplicationDbContext> _mockApplicationDbContext;
    private readonly Mock<DbSet<TodoItem>> _mockTodoItem;
    private readonly Fixture _fixture;

    public EntityRepositoryTests()
    {
        _mockApplicationDbContext = new Mock<IApplicationDbContext>(MockBehavior.Strict);
        _mockTodoItem = new Mock<DbSet<TodoItem>>(MockBehavior.Strict);
        _fixture = new Fixture();
    }

    //[Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // arrange
        var data = new List<TodoItem>()
        {
            new TodoItem() { Id = Guid.NewGuid(), Description = "Something desc", Title = "Something title" }
        };

        _mockApplicationDbContext.Setup(c => c.TodoItems.ToListAsync(CancellationToken.None)).Returns(Task.FromResult(data));
        _mockApplicationDbContext.Setup(c => c.Set<TodoItem>()).Returns(_mockTodoItem.Object);
        
        var repository = new EntityRepository<TodoItem>(_mockApplicationDbContext.Object);
        
        // act
        var items = await repository.GetAllAsync();
        
        // assert
        Assert.Equal(data.Count(), items.Count);
    }
}