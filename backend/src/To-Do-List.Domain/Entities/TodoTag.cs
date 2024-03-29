﻿using System.Drawing;
using System.Text.Json.Serialization;

namespace To_Do_List.Domain.Entities;

public class TodoTag : BaseAuditableEntity
{
    public string Name { get; set; }
    public KnownColor Color { get; set; }
    public Guid UserId { get; set; }
    public Guid TodoItemId { get; set; }
    
    public TodoItem? TodoItem { get; set; }
}