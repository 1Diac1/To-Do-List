using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.DTOs;
using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Repositories;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;
using To_Do_List.Domain.Models;

namespace To_Do_List.Application.Services;

public class TodoItemService : EntityRepository<TodoItem>, ITodoItemService
{
    private readonly IMapper _mapper;
    
    public TodoItemService(IApplicationDbContext dbContext, IMapper mapper) 
        : base(dbContext)
    {
        _mapper = mapper;
    }

    public override async Task<IReadOnlyList<TodoItem>> GetAllAsync()
    {
        return await DbContext.TodoItems
            .Include(item => item.Tags)
            .ToListAsync();
    }

    public override async Task<TodoItem> GetByIdAsync(Guid id)
    {
        return await DbContext.TodoItems
            .Include(item => item.Tags)
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName)
    {
        return await base
            .GetAllAsync(item => item.Tags.Any(tag => tag.Name == tagName));
    }

    public async Task<TodoItem> AddAsync(TodoItem entity, ApplicationUser user, bool autoSave = true)
    {
        entity.Type = TodoStatusTask.InProgress;
        entity.TodoPriorityLevel = TodoPriorityLevel.Low;
        entity.UserId = user.Id;
        entity.Created = DateTime.Now;
        entity.Modified = DateTime.Now;

        return await base.AddAsync(entity, autoSave);
    }

    public async Task<TodoItem> GetAsync(Guid id, ApplicationUser user)
    {
        return await DbContext.TodoItems
            .Include(item => item.Tags)
            .FirstOrDefaultAsync(item => item.Id == id && item.UserId == user.Id);
    }

    public async Task UpdatePatchAsync(TodoItem entity, JsonPatchDocument<TodoItemForUpdateStatusDTO> document)
    {
        var entityToPatch = _mapper.Map<TodoItemForUpdateStatusDTO>(entity);
        
        document.ApplyTo(entityToPatch);

        if (Enum.IsDefined(typeof(TodoStatusTask), entityToPatch.Type) is false)
            throw new BadRequestException("Value is not in the range");

        _mapper.Map(entityToPatch, entity);

        await SaveAsync();
    }

    public override async Task UpdateAsync(TodoItem entity, bool autoSave = true)
    {
        entity.Modified = DateTime.Now;

        await base.UpdateAsync(entity, autoSave);
    }

    public async Task DeleteAsync(Guid id, ApplicationUser user, bool autoSave = true)
    {
        var todo = await base
            .GetAsync(item => item.Id == id && item.UserId == user.Id);
        
        await base.DeleteAsync(todo, autoSave);
    }
}