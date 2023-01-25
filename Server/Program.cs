global using ToDo.Server.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(context.Configuration["Elastic:Uri"]))
    {
        IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace('.', '-')}-{DateTime.UtcNow:yyyy-MM}}",
        AutoRegisterTemplate = true,
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
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
    app.UseSerilogRequestLogging(config =>
    {
        config.Enrich
    });

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
//es01  | {"@timestamp":"2023-01-25T05:38:03.608Z", "log.level":"ERROR", "message":"fatal exception while booting Elasticsearch", "ecs.version": "1.2.0","service.name":"ES_ECS","event.dataset":"elasticsearch.server","process.thread.name":"main","log.logger":"org.elasticsearch.bootstrap.Elasticsearch","elasticsearch.node.name":"es01","elasticsearch.cluster.name":"es-docker-cluster","error.type":"java.lang.IllegalArgumentException","error.message":"unknown setting [cluster.initial_master_node] please check that any required plugins are installed, or check the breaking changes documentation for removed settings","error.stack_trace":"java.lang.IllegalArgumentException: unknown setting [cluster.initial_master_node] please check that any required plugins are installed, or check the breaking changes documentation for removed settings\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.common.settings.AbstractScopedSettings.validate(AbstractScopedSettings.java:561)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.common.settings.AbstractScopedSettings.validate(AbstractScopedSettings.java:507)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.common.settings.AbstractScopedSettings.validate(AbstractScopedSettings.java:477)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.common.settings.AbstractScopedSettings.validate(AbstractScopedSettings.java:447)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.common.settings.SettingsModule.<init>(SettingsModule.java:151)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.common.settings.SettingsModule.<init>(SettingsModule.java:56)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.node.Node.<init>(Node.java:472)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.node.Node.<init>(Node.java:322)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.bootstrap.Elasticsearch$2.<init>(Elasticsearch.java:214)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.bootstrap.Elasticsearch.initPhase3(Elasticsearch.java:214)\n\tat org.elasticsearch.server@8.6.0/org.elasticsearch.bootstrap.Elasticsearch.main(Elasticsearch.java:67)\n"}

