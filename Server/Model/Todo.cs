global using ToDo.Shared;

namespace ToDo.Server.Model;

public class Todo
{
    public Todo()
    {
        if (Id == Guid.Empty)
            Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public bool IsComplete { get; set; }
    public DateTime Date { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public static class TodoMappingExtensions
{
    public static TodoItem AsTodoItem(this Todo todo)
    {
        return new()
        {
            Id = todo.Id,
            Title = todo.Title,
            IsComplete = todo.IsComplete,
            Date = todo.Date
        };
    }
}
