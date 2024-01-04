using To_Do_List.Application.Repositories;
using To_Do_List.Infrastructure.Data;
using To_Do_List.Domain.Entities;
using AutoFixture;

namespace To_Do_List.Application.IntegrationTests.Repositories;

public class EntityRepositoryTests
{
    private readonly Fixture _fixture;

    public EntityRepositoryTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    { 
        // arrange
        var repository = await GetRepositoryAsync();
        
        // act
        var entities = await repository.GetAllAsync();
        
        // assert
        Assert.Equal(1, entities.Count);
        Assert.Contains("Something title", entities.Select(c => c.Title));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntityById()
    {
        // arrange
        var repository = await GetRepositoryAsync();
        
        // act
        var entity = await repository.GetByIdAsync(Guid.Parse("620ba1b1-d8a3-4570-a0a4-88dbfaa28201"));
        
        // assert
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // arrange
        var repository = await GetRepositoryAsync();
        var todoItem = _fixture.Create<TodoItem>();
        
        // act
        var addedEntity = await repository.AddAsync(todoItem);
        
        // assert
        Assert.NotNull(addedEntity);
        Assert.NotEqual(Guid.Empty, addedEntity.Id);

        var entityInDb = await repository.GetByIdAsync(addedEntity.Id);
        Assert.NotNull(entityInDb);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // arrange
        var repository = await GetRepositoryAsync();
        var todoItem = _fixture.Create<TodoItem>();
        var addedEntity = await repository.AddAsync(todoItem);
        
        // act
        addedEntity.Title = "Updated title";
        await repository.UpdateAsync(addedEntity);
        
        // assert
        var updatedEntity = await repository.GetByIdAsync(addedEntity.Id);
        Assert.NotNull(updatedEntity);
        Assert.Equal("Updated title", updatedEntity.Title);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // arrange
        var repository = await GetRepositoryAsync();
        var todoItem = _fixture.Create<TodoItem>();
        var addedEntity = await repository.AddAsync(todoItem);
        
        // act
        await repository.DeleteAsync(addedEntity);
        
        // assert
        var deletedEntity = await repository.GetByIdAsync(addedEntity.Id);
        Assert.Null(deletedEntity);
    }
    
    private static async Task<EntityRepository<TodoItem>> GetRepositoryAsync()
    {
        var database = await DatabaseInitializer.InitializeAsync(Guid.NewGuid().ToString());
        var context = new ApplicationDbContext(database);
        var repository = new EntityRepository<TodoItem>(context);

        return repository;
    }
}