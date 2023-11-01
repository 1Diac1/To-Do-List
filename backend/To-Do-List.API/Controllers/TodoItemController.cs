using Microsoft.AspNetCore.Mvc;
using To_Do_List.Application.Common.Exceptions;
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
    // TODO: Сделать ApiResponse, и через него делать ответы
    public async Task<ActionResult<IReadOnlyList<TodoItem>>> GetAllAsync()
    {
        var items = await _todoItemService.GetAllAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException("An id can't be null");
        
        var todoItem = await _todoItemService.GetByIdAsync(id);

        if (todoItem is null)
            throw new NotFoundException(nameof(TodoItem), id);
        
        return Ok(todoItem);
    }
    
    
}