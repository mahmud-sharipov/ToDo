using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDo.Api.Data;

namespace ToDo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly TodoContext db;

    public TodosController(TodoContext context)
    {
        db = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemResponseDto>>> Get(
        [FromQuery] bool? isCompleted,
        [FromQuery] bool? hasDueDate,
        [FromQuery] string orderBy = "DueDate",
        [FromQuery] bool orderByDescending = false)
    {
        IQueryable<Todo> query = db.Todos;

        //OrderBy
        {
            Expression<Func<Todo, DateTime?>> exp = orderBy == "DueDate" ? t => t.Date : t => t.CreatedAt;
            query = orderByDescending ? query.OrderBy(exp) : query.OrderByDescending(exp);
        }

        if (isCompleted.HasValue)
            query = query.Where(t => t.IsComplete == isCompleted.HasValue);

        if (hasDueDate.HasValue)
            query = hasDueDate.Value ? query.Where(t => t.Date != null) : query.Where(t => t.Date == null);

        return await query.AsNoTracking().Select(t => t.AsTodoItem()).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemResponseDto>> Get(int id)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo == null)
            return NotFound();

        return Ok(todo.AsTodoItem());
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemResponseDto>> Post([FromBody] TodoItemRequestDto newTodo)
    {
        var todo = new Todo
        {
            Title = newTodo.Title,
            Date = newTodo.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        return Created($"/todos/{todo.Id}", todo.AsTodoItem());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, TodoItemRequestDto todo)
    {
        var rowsAffected = await db.Todos.Where(t => t.Id == id)
                                         .ExecuteUpdateAsync(updates =>
                                            updates.SetProperty(t => t.IsComplete, todo.IsComplete)
                                                   .SetProperty(t => t.Title, todo.Title)
                                                   .SetProperty(t => t.Date, todo.DueDate)
                                                   .SetProperty(t => t.UpdatedAt, DateTime.UtcNow));

        return rowsAffected == 0 ? NotFound() : Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var rowsAffected = await db.Todos.Where(t => t.Id == id).ExecuteDeleteAsync();
        return rowsAffected == 0 ? NotFound() : Ok();
    }
}
