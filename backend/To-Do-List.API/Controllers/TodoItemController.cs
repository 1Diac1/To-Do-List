using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Application.Common.Filters;
using Microsoft.AspNetCore.Authorization;
using To_Do_List.Application.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Identity;
using To_Do_List.Application.DTOs;
using To_Do_List.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.API.Contracts;
using To_Do_List.Domain.Models;
using To_Do_List.Domain.Enums;
using AutoMapper;

namespace To_Do_List.API.Controllers;

[ApiController]
[Authorize]
public class TodoItemController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public TodoItemController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _todoItemService = todoItemService;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet(ApiRoutes.TodoItem.GetAll)]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetAllAsync()
    {
        var items = await _todoItemService.GetAllAsync();
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(items);
        
        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet(ApiRoutes.TodoItem.GetById)]
    public async Task<DataResponse<TodoItemDTO>> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException(ErrorMessages.CantBeNull);
        
        var todoItem = await _todoItemService.GetByIdAsync(id);

        if (todoItem is null)
            throw new NotFoundException(nameof(TodoItem), id);

        var mappedItem = _mapper.Map<TodoItemDTO>(todoItem);

        return DataResponse<TodoItemDTO>.Success(mappedItem);
    }

    [HttpGet(ApiRoutes.TodoItem.GetTodoItemsByTagName)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByTagNameAsync(string tagName)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            throw new BadRequestException(ErrorMessages.CantBeNull);
        
        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetTodoItemsByTagNameAsync(tagName, user);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet(ApiRoutes.TodoItem.GetTodoItemsByStatusTask)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByStatusTaskAsync(TodoStatusTask status)
    {
        var isDefined = Enum.IsDefined(typeof(TodoStatusTask), status);
        
        if (isDefined is false)
            throw new BadRequestException(ErrorMessages.InvalidValue, typeof(TodoStatusTask));

        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetTodoItemsByStatusTaskAsync(status, user);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet(ApiRoutes.TodoItem.GetTodoItemsByDueDate)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByDueDateAsync(DateTime dueDate)
    {
        if (dueDate < DateTime.Now)
            throw new BadRequestException(ErrorMessages.CantBePast, typeof(DateTime));

        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetTodoItemsByDueDateAsync(dueDate, user);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet(ApiRoutes.TodoItem.GetTodoItemsStatistics)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<DataResponse<TodoItemStatistics>> GetTodoItemsStatisticsAsync()
    {
        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var stats = await _todoItemService.GetTodoItemsStatisticsAsync(user);

        return DataResponse<TodoItemStatistics>.Success(stats);
    }

    [HttpGet(ApiRoutes.TodoItem.GetTodoItemsByUserId)]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetTodoItemsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new BadRequestException(ErrorMessages.CantBeNull, typeof(Guid));

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            throw new NotFoundException(nameof(ApplicationUser), userId);
        
        var todoItems = await _todoItemService.GetTodoItemsByUserIdAsync(user.Id);
        var mappedItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedItems);
    }

    [HttpGet(ApiRoutes.TodoItem.GetTodoTags)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<DataResponse<IReadOnlyList<TodoTagDTO>>> GetTodoTagsAsync()
    {
        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoTags = await _todoItemService.GetTodoTagsAsync(user);
        var mappedTodoTags = _mapper.Map<IReadOnlyList<TodoTagDTO>>(todoTags);

        return DataResponse<IReadOnlyList<TodoTagDTO>>.Success(mappedTodoTags);
    }

    [HttpGet(ApiRoutes.TodoItem.GetSortedIncompleteTodoItems)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<DataResponse<IReadOnlyList<TodoItemDTO>>> GetSortedIncompleteTodoItemsAsync()
    {
        var user = new ApplicationUser() { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var todoItems = await _todoItemService.GetSortedIncompleteTodoItemsAsync(user);
        var mappedTodoItems = _mapper.Map<IReadOnlyList<TodoItemDTO>>(todoItems);

        return DataResponse<IReadOnlyList<TodoItemDTO>>.Success(mappedTodoItems);
    }

    [HttpPost(ApiRoutes.TodoItem.Create)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<BaseResponse> CreateAsync([FromBody] TodoItemDTO entity)
    {
        if (entity is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, typeof(TodoItemDTO));

        var mappedEntity = _mapper.Map<TodoItem>(entity);

        var user = new ApplicationUser { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };
        
        await _todoItemService.AddAsync(mappedEntity, user);
        
        return BaseResponse.Success();
    }

    [HttpPut(ApiRoutes.TodoItem.Update)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<BaseResponse> UpdateAsync([FromBody] TodoItemDTO entity, Guid id)
    {
        if (entity is null)
            throw new BadRequestException(ErrorMessages.CantBeNull, typeof(TodoItemDTO));

        var user = new ApplicationUser { Id = Guid.Parse("BEE42B62-692D-4D4C-9C1D-700A76944135") };
        
        var entityToUpdate = await _todoItemService.GetAsync(id, user);

        if (entityToUpdate is null)
            throw new NotFoundException(nameof(TodoItem), id);

        _mapper.Map(entity, entityToUpdate);
        
        await _todoItemService.UpdateAsync(entityToUpdate);

        return BaseResponse.Success();
    }

    [HttpPatch(ApiRoutes.TodoItem.Patch)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<BaseResponse> UpdatePatchAsync(Guid id, [FromBody] JsonPatchDocument<TodoItemForPatchDTO> status)
    {
        if (id == Guid.Empty)
            throw new BadRequestException(ErrorMessages.CantBeNull, typeof(Guid));

        var user = new ApplicationUser { Id = Guid.Parse("8B3CA57D-77ED-423E-AED4-1CE2737D83E5") };

        var entity = await _todoItemService.GetAsync(id, user);
        
        if (entity is null)
            throw new NotFoundException(nameof(TodoItem), id);

        entity.Modified = DateTime.Now;
        
        await _todoItemService.UpdatePatchAsync<TodoItemForPatchDTO>(entity, status);
        
        return BaseResponse.Success();
    }

    [HttpDelete(ApiRoutes.TodoItem.Delete)]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public async Task<BaseResponse> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException(ErrorMessages.CantBeNull, typeof(Guid));
        
        var entity = await _todoItemService.GetByIdAsync(id);

        if (entity is null)
            throw new NotFoundException(nameof(TodoItem), id);

        var user = new ApplicationUser { Id = Guid.Parse("64FB4F03-13E8-4804-8E4B-A3388EF7BBB3") };
        
        await _todoItemService.DeleteAsync(id, user);

        return BaseResponse.Success();
    }
}