global using ToDo.Api.Model;
using Microsoft.EntityFrameworkCore;
using ToDo.Api.Model;

namespace ToDo.Api.Data;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Todo> Todos => Set<Todo>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<Todo>()
        //       .HasOne<TodoUser>()
        //       .WithMany()
        //       .HasForeignKey(t => t.OwnerId)
        //       .HasPrincipalKey(u => u.UserName);

        base.OnModelCreating(builder);
    }
}