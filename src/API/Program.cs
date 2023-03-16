using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDo.Api.Data;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, config) =>
        {
            config.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console()
            //.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(context.Configuration["Elastic:Uri"]))
            //{
            //    IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace('.', '-')}-{DateTime.UtcNow:yyyy-MM}}",
            //    AutoRegisterTemplate = true,
            //    NumberOfShards = 2,
            //    NumberOfReplicas = 1
            //})
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .ReadFrom.Configuration(context.Configuration);
        });

        //var t = Host.CreateDefaultBuilder(args)
        //    .UseSerilog();


        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Add services to the container.
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
            var dbCongig = builder.Configuration.GetSection("Database");
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
    }
}

