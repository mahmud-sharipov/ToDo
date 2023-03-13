using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ToDo.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        var t = builder.Configuration["ServerUrl"];
        Console.WriteLine(t);
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(@"http://api.mahmud.20.219.235.7.nip.io/api/") });

        await builder.Build().RunAsync();
    }
}