namespace To_Do_List.Domain.Models;

public class TodoItemStatistics
{
    public int TotalTasks { get; set; }
    public int TotalInProgressTasks { get; set; }
    public int TotalPendingTasks { get; set; }
    public int TotalCompletedTasks { get; set; }
    public double? AverageTaskCompletionTime { get; set; }
    public Dictionary<TodoPriorityLevel, int> PriorityCounts { get; set; }
    public Dictionary<string, int> TagsCounts { get; set; }
}