using Microsoft.AspNetCore.Mvc;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Entities;

namespace To_Do_List.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;

    public TodoItemController(ITodoItemService todoItemService)
    {
        this._todoItemService = todoItemService;
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
}