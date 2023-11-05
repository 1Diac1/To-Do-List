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

    public async Task<IReadOnlyList<TodoItem>> GetTodoItemsByTagNameAsync(string tagName, ApplicationUser user)
    {
        return await base.DbContext.TodoItems
            .Include(item => item.Tags)
            .Where(item => item.Tags.Any(tag => tag.Name == tagName) &&
                           item.UserId == user.Id)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> GetTodoItemsByStatusTaskAsync(TodoStatusTask status, ApplicationUser user)
    {
        return await base.DbContext.TodoItems
            .Include(item => item.Tags)
            .Where(item => item.UserId == user.Id &&
                           item.StatusTask == status)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> GetTodoItemsByDueDateAsync(DateTime date, ApplicationUser user)
    {
        return await base.DbContext.TodoItems
            .Include(item => item.Tags)
            .Where(item => item.DueDate.Date == date.Date)
            .ToListAsync();
    }

    public async Task<TodoItemStatistics> GetTodoItemsStatisticsAsync(ApplicationUser user)
    {
        var todoItems = await base.DbContext.TodoItems
            .Include(item => item.Tags)
            .Where(item => item.UserId == user.Id)
            .ToListAsync();

        var completedTasks = todoItems
            .Where(item => item is { StatusTask: TodoStatusTask.Completed, CompletionDate: not null })
            .ToList();

        var priorityCounts = todoItems.GroupBy(item => item.PriorityLevel)
            .ToDictionary(g => g.Key, g => g.Count());
        
        var tagsCounts = new Dictionary<string, int>();
        
        foreach (var tags in todoItems.Select(item => item.Tags))
            foreach (var tag in tags)
                tagsCounts.TryAdd(tag.Name, todoItems.Count(item => item.Tags.Any(t => t.Name == tag.Name)));

        var totalCompletionTime = completedTasks
            .Sum(t => (t.CompletionDate - t.Created)?.TotalHours);
        
        var statistics = new TodoItemStatistics
            {
                TotalTasks = todoItems.Count,
                TotalInProgressTasks = todoItems.Count(t => t.StatusTask == TodoStatusTask.InProgress),
                TotalPendingTasks = todoItems.Count(t => t.StatusTask == TodoStatusTask.Pending),
                TotalCompletedTasks = todoItems.Count(t => t.StatusTask == TodoStatusTask.Completed),
                AverageTaskCompletionTime = completedTasks.Count > 0 ? totalCompletionTime / completedTasks.Count : 0,
                PriorityCounts = priorityCounts,
                TagsCounts = tagsCounts
            };

        return statistics;
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

    public async Task UpdatePatchAsync<T>(TodoItem entity, JsonPatchDocument<T> patchDocument)
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

        entity.CompletionDate = entity.StatusTask is TodoStatusTask.Completed
            ? DateTime.Now 
            : null;

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