global using ToDo.Shared;

namespace ToDo.Api.Model;

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
    public DateTime? Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public static class TodoMappingExtensions
{
    public static TodoItemResponseDto AsTodoItem(this Todo todo)
    {
        return new()
        {
            Id = todo.Id,
            Title = todo.Title,
            IsComplete = todo.IsComplete,
            DueDate = todo.Date,
            CreatedAt = todo.CreatedAt
        };
    }
}
