using System.Drawing;
using To_Do_List.Domain.Enums;

namespace To_Do_List.Application.DTOs;

public class TodoTagDTO
{
    public string Name { get; set; }
    public KnownColor Color { get; set; }
}