using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Server.Controllers;

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
    public async Task<ActionResult<IEnumerable<TodoItem>>> Get([FromQuery] bool? completed)
    {
        IQueryable<Todo> query = db.Todos.OrderBy(t => t.Date).ThenBy(t => t.Title);
        if (completed.HasValue)
            query = query.Where(t => t.IsComplete == completed.Value);
        return await query.Select(t => t.AsTodoItem()).AsNoTracking().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> Get(int id)
    {
        var todo = await db.Todos.FindAsync(id);
        if (todo == null)
            return NotFound();

        return Ok(todo.AsTodoItem());
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Post([FromBody] TodoItem newTodo)
    {
        var todo = new Todo
        {
            Title = newTodo.Title,
            Date = newTodo.Date,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        return Created($"/todos/{todo.Id}", todo.AsTodoItem());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, TodoItem todo)
    {
        if (id != todo.Id)
        {
            return BadRequest();
        }

        var rowsAffected = await db.Todos.Where(t => t.Id == id)
                                         .ExecuteUpdateAsync(updates =>
                                            updates.SetProperty(t => t.IsComplete, todo.IsComplete)
                                                   .SetProperty(t => t.Title, todo.Title)
                                                   .SetProperty(t => t.UpdatedAt, DateTimeOffset.Now));

        return rowsAffected == 0 ? NotFound() : Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var rowsAffected = await db.Todos.Where(t => t.Id == id).ExecuteDeleteAsync();
        return rowsAffected == 0 ? NotFound() : Ok();
    }
}
