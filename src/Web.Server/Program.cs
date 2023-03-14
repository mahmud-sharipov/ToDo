using ToDo.Web.Server;

var builder = WebApplication.CreateBuilder(args);

// Add razor pages so we can render the Blazor WASM todo component
builder.Services.AddRazorPages();

// Add the forwarder to make sending requests to the backend easier
builder.Services.AddHttpForwarder();

var todoUrl = builder.Configuration.GetServiceUri("todoapi")?.ToString() ??
              builder.Configuration["TodoApiUrl"] ??
              throw new InvalidOperationException("Todo API URL is not configured");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapTodos(todoUrl);

app.MapFallbackToPage("/_Host");

app.Run();
