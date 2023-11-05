using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Application.DTOs;
using To_Do_List.Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;
using AutoMapper;
using FluentValidation;
using To_Do_List.Domain.Enums;

namespace To_Do_List.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    // TODO: вынести в константы стандартные ошибки BadRequest
    // TODO: вынести все валидации в фильтры
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public TodoItemController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _todoItemService = todoItemService;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetAllAsync()
    {
        var items = await _todoItemService.GetAllAsync();
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(items);
        
        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet("{id}")]
    public async Task<DataResponse<TodoItemDTO>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException("An id can't be null");
        
        var todoItem = await _todoItemService.GetByIdAsync(id);

        if (todoItem is null)
            throw new NotFoundException(nameof(TodoItem), id);

        var mappedItem = _mapper.Map<TodoItemDTO>(todoItem);

        return DataResponse<TodoItemDTO>.Success(mappedItem);
    }

    [HttpGet("tag/{tagName}")]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByTagNameAsync(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            throw new BadRequestException("Tag can't be null");
        
        // var user (Найти настоящего пользователя)

        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetTodoItemsByTagNameAsync(tagName, user);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet("status/{status}")]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByStatusTaskAsync(TodoStatusTask status)
    {
        var isDefined = Enum.IsDefined(typeof(TodoStatusTask), status);
        
        if (isDefined is false)
            throw new BadRequestException("Invalid value for status task");
        
        // (проверка пользователя)

        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetTodoItemsByStatusTaskAsync(status, user);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet("due/{dueDate:datetime}")]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByDueDateAsync(DateTime dueDate)
    {
        if (dueDate < DateTime.Now)
            throw new BadRequestException("Date can't be past");
        
        // (проверка пользователя)

        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetTodoItemsByDueDateAsync(dueDate, user);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet("stats")]
    public async Task<DataResponse<TodoItemStatistics>> GetTodoItemsStatisticsAsync()
    {
        // (проверка пользователя)

        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var stats = await _todoItemService.GetTodoItemsStatisticsAsync(user);

        return DataResponse<TodoItemStatistics>.Success(stats);
    }
    
    [HttpPost]
    public async Task<BaseResponse> CreateAsync([FromBody] TodoItemDTO entity)
    {
        if (entity is null)
            throw new BadRequestException("An item can't be null");

        //var user = await _userManager.GetUserAsync(User);

        //if (user is null)
        //    throw new BadRequestException("Unauthorized");

        var mappedEntity = _mapper.Map<TodoItem>(entity);

        var user = new ApplicationUser { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };
        
         await _todoItemService.AddAsync(mappedEntity, user);
        
        return BaseResponse.Success();
    }

    [HttpPut("{id}")]
    public async Task<BaseResponse> UpdateAsync([FromBody] TodoItemDTO entity, Guid id)
    {
        if (entity is null)
            throw new BadRequestException("An item can't be null");

        //var user = await _userManager.GetUserAsync(User);

        //if (user is null)
        //    throw new BadRequestException("Unauthorized");

        var user = new ApplicationUser { Id = Guid.Parse("BEE42B62-692D-4D4C-9C1D-700A76944135") };
        
        var entityToUpdate = await _todoItemService.GetAsync(id, user);

        if (entityToUpdate is null)
            throw new NotFoundException(nameof(TodoItem), id);

        _mapper.Map(entity, entityToUpdate);
        
        await _todoItemService.UpdateAsync(entityToUpdate);

        return BaseResponse.Success();
    }

    [HttpPatch("{id:guid}")]
    public async Task<BaseResponse> UpdatePatchAsync(Guid id, [FromBody] JsonPatchDocument<TodoItemForPatchDTO> status)
    {
        if (id == Guid.Empty)
            throw new BadRequestException("An id can't be null");

        var user = new ApplicationUser { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var entity = await _todoItemService.GetAsync(id, user);
        
        if (entity is null)
            throw new NotFoundException(nameof(TodoItem), id);

        entity.Modified = DateTime.Now;
        
        await _todoItemService.UpdatePatchAsync<TodoItemForPatchDTO>(entity, status);
        
        return BaseResponse.Success();
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException("An id can't be null");
        
        //var user = await _userManager.GetUserAsync(User);

        //if (user is null)
        //    throw new BadRequestException("Unauthorized");
        
        var entity = await _todoItemService.GetByIdAsync(id);

        if (entity is null)
            throw new NotFoundException(nameof(TodoItem), id);

        var user = new ApplicationUser  
        {
            Id = Guid.Parse("64FB4F03-13E8-4804-8E4B-A3388EF7BBB3")
        };
        
        await _todoItemService.DeleteAsync(id, user);

        return BaseResponse.Success();
    }
}