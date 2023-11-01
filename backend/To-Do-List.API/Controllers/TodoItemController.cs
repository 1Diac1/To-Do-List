using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;

namespace To_Do_List.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    // TODO: сделать Requests и DTO
    // TODO: прочитать и сделать HttpPatch
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TodoItemController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager)
    {
        this._todoItemService = todoItemService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<DataResponse<IReadOnlyList<TodoItem>>> GetAllAsync()
    {
        var items = await _todoItemService.GetAllAsync();

        return DataResponse<IReadOnlyList<TodoItem>>.Success(items);
    }

    [HttpGet("{id}")]
    public async Task<BaseResponse> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException("An id can't be null");
        
        var todoItem = await _todoItemService.GetByIdAsync(id);

        if (todoItem is null)
            throw new NotFoundException(nameof(TodoItem), id);

        return BaseResponse.Success();
    }

    [HttpPost]
    public async Task<BaseResponse> CreateAsync([FromBody] TodoItem entity)
    {
        if (entity is null)
            throw new BadRequestException("An item can't be null");

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            throw new BadRequestException("Unauthorized");
        
        var createdEntity = await _todoItemService.AddAsync(entity, user);
        
        return BaseResponse.Success();
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateAsync([FromBody] TodoItem entity)
    {
        if (entity is null)
            throw new BadRequestException("An item can't be null");

        var entityToUpdate = await _todoItemService.GetByIdAsync(entity.Id);

        if (entityToUpdate is null)
            throw new NotFoundException(nameof(TodoItem), entity.Id);

        await _todoItemService.UpdateAsync(entityToUpdate);

        return BaseResponse.Success();
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse> DeleteAsync(Guid id)
    {
        var entity = await _todoItemService.GetByIdAsync(id);

        if (entity is null)
            throw new NotFoundException(nameof(TodoItem), id);

        await _todoItemService.DeleteAsync(entity);

        return BaseResponse.Success();
    }
}