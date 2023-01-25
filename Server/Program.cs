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
    var dbCongig = builder.Configuration.GetSection("Database");
    Console.WriteLine($"Host={dbCongig["Server"]};Database={dbCongig["Name"]};Username={dbCongig["User"]};Password={dbCongig["Password"]}");
    builder.Services.AddDbContext<TodoContext>(options =>
    {
        options.UseNpgsql($"Host={dbCongig["Server"]};Database={dbCongig["Name"]};Username={dbCongig["User"]};Password={dbCongig["Password"]}");
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
    app.UseAuthorization();
    app.MapControllers();
}

app.Run();
