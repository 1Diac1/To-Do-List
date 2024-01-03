namespace To_Do_List.API.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;

    public static class TodoItem
    {
        private const string TodoItemBase = Base + "/todoitem";
        public const string GetAll = TodoItemBase;
        public const string GetById = TodoItemBase + "/{id}";
        public const string GetTodoItemsByTagName = TodoItemBase + "/tag/{tagName}";
        public const string GetTodoItemsByStatusTask = TodoItemBase + "/status/{status}";
        public const string GetTodoItemsByDueDate = TodoItemBase + "/due/{dueDate:datetime}";
        public const string GetTodoItemsStatistics = TodoItemBase + "/stats";
        public const string GetTodoItemsByUserId = TodoItemBase + "/user/{userId}";
        public const string GetTodoTags = TodoItemBase + "/tags";
        public const string GetSortedIncompleteTodoItems = TodoItemBase + "/sorted/due";
        public const string Create = TodoItemBase;
        public const string Update = TodoItemBase + "/{id}";
        public const string Patch = TodoItemBase + "/{id:guid}";
        public const string Delete = TodoItemBase + "/{id}";
    }
}