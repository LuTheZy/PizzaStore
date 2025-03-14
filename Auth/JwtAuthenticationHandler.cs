using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace PizzaStore.Auth;

public class JwtAuthenticationHandler
{
    private readonly RequestDelegate _next;
    private readonly IJwtService _jwtService;

    public JwtAuthenticationHandler(RequestDelegate next, IJwtService jwtService)
    {
        _next = next;
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            try
            {
                var claims = _jwtService.ValidateToken(token);
                context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            }
            catch
            {
                // Token validation failed
            }
        }

        await _next(context);
    }
}
