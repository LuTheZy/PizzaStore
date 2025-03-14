using System.Security.Claims;

namespace PizzaStore.Auth;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() is not null)
        {
            if (context.User.Identity is not { IsAuthenticated: true })
            {
                context.Response.StatusCode = 401;
                return;
            }
        }

        await _next(context);
    }
}
