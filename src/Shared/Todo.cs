namespace ToDo.Shared;

public class TodoItemResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TodoItemRequestDto
{
    public string Title { get; set; }
    public bool IsComplete { get; set; }
    public DateTime? DueDate { get; set; }
}
