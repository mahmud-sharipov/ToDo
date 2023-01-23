global using ToDo.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCors();
    builder.Services.AddDbContext<TodoContext>(options =>
    {
        //var connectionString = builder.Configuration.GetConnectionString("Todos") ?? "Data Source=Todos.db";
        options.UseNpgsql("Host=localhost;Database=ToDo;Username=admin;Password=admin");
        //options.UseInMemoryDatabase("ToDo");
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        //app.UseHsts();
    }

    app.UseCors(option =>
    {
        option.AllowAnyHeader();
        option.AllowAnyMethod();
        option.AllowAnyOrigin();
    });
    app.UseHttpsRedirection();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

app.Run();
