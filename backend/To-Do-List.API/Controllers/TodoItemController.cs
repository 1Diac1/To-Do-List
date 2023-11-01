﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Helpers;
using To_Do_List.Application.DTOs;
using To_Do_List.Application.Interfaces;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Models;
using AutoMapper;

namespace To_Do_List.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    // TODO: прочитать и сделать HttpPatch
    // TODO: вынести получения пользователя в отдельный фильтр или атрибут
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public TodoItemController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        this._todoItemService = todoItemService;
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

    [HttpPost]
    public async Task<BaseResponse> CreateAsync([FromBody] TodoItemDTO entity)
    {
        if (entity is null)
            throw new BadRequestException("An item can't be null");

        //var user = await _userManager.GetUserAsync(User);

        //if (user is null)
        //    throw new BadRequestException("Unauthorized");

        var mappedEntity = _mapper.Map<TodoItem>(entity);
    
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid()
        };
        
         await _todoItemService.AddAsync(mappedEntity, user);
        
        return BaseResponse.Success();
    }

    [HttpPut]
    public async Task<BaseResponse> UpdateAsync([FromBody] TodoItemDTO entity, Guid id)
    {
        if (entity is null)
            throw new BadRequestException("An item can't be null");

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            throw new BadRequestException("Unauthorized");
        
        var entityToUpdate = await _todoItemService.GetByIdAsync(id);

        if (entityToUpdate is null)
            throw new NotFoundException(nameof(TodoItem), id);

        entityToUpdate = _mapper.Map<TodoItem>(entity);
        
        await _todoItemService.UpdateAsync(entityToUpdate);

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