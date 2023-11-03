using Microsoft.EntityFrameworkCore;
using To_Do_List.Application.Interfaces;
using To_Do_List.Application.Repositories;
using To_Do_List.Domain.Common;
using To_Do_List.Domain.Entities;
using To_Do_List.Domain.Enums;
using To_Do_List.Domain.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using To_Do_List.Application.Common.Exceptions;
using To_Do_List.Application.Common.Validators;

namespace To_Do_List.Application.Services;

public class TodoItemService : EntityRepository<TodoItem>, ITodoItemService
{
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;
    
    public TodoItemService(IApplicationDbContext dbContext, IMapper mapper, IValidationService validationService) 
        : base(dbContext)
    {
        _mapper = mapper;
        _validationService = validationService;
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
        entity.StatusTask = TodoStatusTask.InProgress;
        entity.PriorityLevel = TodoPriorityLevel.Low;
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

    public async Task UpdatePatchAsync<T>(TodoItem entity, JsonPatchDocument patchDocument)
        where T : BaseEntityDTO
    {
        var mappedEntity = _mapper.Map<T>(entity);
        
        patchDocument.ApplyTo(mappedEntity);

        var validatorForType = _validationService.GetValidatorForType<T>();

        if (validatorForType is null)
            throw new BadRequestException($"Service for type {typeof(T)} was not found");
        
        var validation = await validatorForType.ValidateAsync(mappedEntity);

        if (validation.IsValid is false)
            throw new ValidationException(validation.Errors);
        
        _mapper.Map(mappedEntity, entity);
        
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