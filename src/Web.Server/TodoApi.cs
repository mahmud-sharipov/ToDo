using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net;
using Yarp.ReverseProxy.Forwarder;

namespace ToDo.Web.Server;

public static class TodoApi
{
    public static RouteGroupBuilder MapTodos(this IEndpointRouteBuilder routes, string todoUrl)
    {
        var group = routes.MapGroup("/todos");

        var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
        {
            UseProxy = false,
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.None,
            UseCookies = false,
            ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
            ConnectTimeout = TimeSpan.FromSeconds(15),
        });
        var transformer = HttpTransformer.Default;
        var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };

        group.Map("{*path}", async (IHttpForwarder forwarder, HttpContext httpContext) =>
        {
            var error = await forwarder.SendAsync(httpContext, todoUrl, httpClient, requestConfig, transformer);
            // Check if the operation was successful
            if (error != ForwarderError.None)
            {
                var errorFeature = httpContext.GetForwarderErrorFeature();
                var exception = errorFeature.Exception;
            }
        });

        return group;
    }
}
