using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Services;
using To_Do_List.Infrastructure.Data;
using Microsoft.AspNetCore.JsonPatch;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Models;
using To_Do_List.Domain.Enums;
using AutoMapper;
using AutoFixture;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using To_Do_List.Application.Mappings;
using To_Do_List.Domain.Entities;

namespace To_Do_List.Application.IntegrationTests.Services;

public class TodoItemServiceTests
{
    private readonly TodoItemService _todoItemService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IValidationService> _mockValidationService;

    public TodoItemServiceTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockValidationService = new Mock<IValidationService>();

        var databaseName = Guid.NewGuid().ToString();
        var dbContextOptions = DatabaseInitializer.GetDbContextOptions(databaseName);
        var context = new ApplicationDbContext(dbContextOptions);

        _todoItemService = new TodoItemService(context, _mockMapper.Object, _mockValidationService.Object);
        DatabaseInitializer.InitializeAsync(databaseName);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTodoItemsWithTags()
    {
        // act
        var items = await _todoItemService.GetAllAsync();
        
        // assert
        Assert.NotNull(items);
        Assert.Single(items.Select(i => i.Tags));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTodoItemWithTags()
    {
        // act
        var item = await _todoItemService.GetByIdAsync(Guid.Parse("620ba1b1-d8a3-4570-a0a4-88dbfaa28201"));
        
        // assert
        Assert.NotNull(item);
    }

    [Fact]
    public async Task GetTodoItemsByTagNameAsync_ReturnsTodoItemsByTagName()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetTodoItemsByTagNameAsync("Something name", applicationUser);
        
        // assert
        Assert.NotEmpty(items);
        Assert.Equal(Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8"), items[0].UserId);
    }

    [Fact]
    public async Task GetTodoItemsByStatusTaskAsync_ReturnsTodoItemsByStatusTask()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetTodoItemsByStatusTaskAsync(TodoStatusTask.Completed, applicationUser);
        
        // assert
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task GetTodoItemsByDueDateAsync_ReturnsTodoItemsByDueDate()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetTodoItemsByDueDateAsync(DateTime.Parse("05/01/2024"), applicationUser);
        
        // assert
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task GetSortedIncompleteTodoItemsAsync_ReturnsSortedIncompleteTodoItems()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetSortedIncompleteTodoItemsAsync(applicationUser);
        
        // assert
        Assert.Empty(items);
    }

    [Fact]
    public async Task GetTodoItemsStatisticsAsync_ReturnsTodoItemsStatisticByUser()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetTodoItemsStatisticsAsync(applicationUser);
        
        // assert
        Assert.Equal(1, items.TotalCompletedTasks);
        Assert.Equal(0, items.TotalPendingTasks);
        Assert.Single(items.TagsCounts);
    }

    [Fact]
    public async Task GetTodoItemsByUserIdAsync_ReturnsTodoItemsByUserId()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetTodoItemsByUserIdAsync(applicationUser.Id);
        
        // assert
        Assert.NotEmpty(items);
    }

    [Fact]
    public async Task GetTodoTagsAsync_ReturnsTodoTagsByUser()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var items = await _todoItemService.GetTodoTagsAsync(applicationUser);
        
        // assert
        Assert.Contains("Something name", items.Select(i => i.Name));
    }

    [Fact]
    public async Task GetAsync_ReturnsTodoItemByIdAndUserId()
    {
        // act
        var applicationUser = new ApplicationUser() { Id = Guid.Parse("9f6dfb3f-2c22-4c2c-9b9e-4f96e9f969f8") };
        var itemId = Guid.Parse("620ba1b1-d8a3-4570-a0a4-88dbfaa28201");
        var item = await _todoItemService.GetAsync(itemId, applicationUser);
        
        // assert
        Assert.Equal("Something title", item.Title);
        Assert.NotEmpty(item.Tags);
    }

    [Fact]
    public async Task UpdatePatchAsync_ShouldUpdateTodoItem()
    {
        // arrange
        var todoItemId = Guid.Parse("620ba1b1-d8a3-4570-a0a4-88dbfaa28201");
        var todoItem = await _todoItemService.GetByIdAsync(todoItemId);
        var todoItemDto = new TodoItemDTO() { Title = todoItem.Title };
        
        var patchDoc = new JsonPatchDocument<TodoItemDTO>();
        patchDoc.Replace(e => e.Title, "Patch updated title");

        // Verifiable() нужен, чтобы в конце проверить с помощью Verify() был ли вызван метод с определенными параметрами
        _mockMapper.Setup(m => m.Map<TodoItemDTO>(It.IsAny<TodoItem>())).Returns(todoItemDto).Verifiable();
        _mockMapper.Setup(m => m.Map(It.IsAny<TodoItemDTO>(), It.IsAny<TodoItem>()))
            .Callback<TodoItemDTO, TodoItem>((dto, entity) => entity.Title = dto.Title);

        var mockValidator = new Mock<IValidator<TodoItemDTO>>();
        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<TodoItemDTO>(), default))
            .ReturnsAsync(new ValidationResult());

        _mockValidationService.Setup(v => v.GetValidatorForType<TodoItemDTO>())
            .Returns(mockValidator.Object);
        
        // act
        await _todoItemService.UpdatePatchAsync(todoItem, patchDoc);
        
        // assert
        _mockMapper.Verify();
        var updatedEntity = await _todoItemService.GetByIdAsync(todoItemId);
        Assert.NotNull(updatedEntity);
        Assert.Equal("Patch updated title", updatedEntity.Title);
    }
}