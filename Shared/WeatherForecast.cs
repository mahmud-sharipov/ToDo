namespace ToDo.Shared;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
    public DateTime Date { get; set; }
}

