using Yarp.ReverseProxy.Forwarder;

namespace ToDo.Web.Server;

public static class TodoApi
{
    public static RouteGroupBuilder MapTodos(this IEndpointRouteBuilder routes, string todoUrl)
    {
        var group = routes.MapGroup("/todos");

        var transform = static async ValueTask (HttpContext context, HttpRequestMessage req) =>
        {
            await ValueTask.CompletedTask;
        };

        var client = new HttpMessageInvoker(new SocketsHttpHandler());

        group.Map("{*path}", async (IHttpForwarder forwarder, HttpContext context) =>
        {
            var err = await forwarder.SendAsync(context, todoUrl, client, transform);
            return Results.Empty;
        });

        return group;
    }
}
