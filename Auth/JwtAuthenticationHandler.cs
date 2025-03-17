using System.Security.Claims;

using PizzaStore.Services.Interfaces;

namespace PizzaStore.Auth
{
    public class JwtAuthenticationHandler
    {
        private readonly RequestDelegate _next;
        private readonly IJwtService _jwtService;
        private readonly ILogger<JwtAuthenticationHandler> _logger;

        public JwtAuthenticationHandler(
            RequestDelegate next,
            IJwtService jwtService,
            ILogger<JwtAuthenticationHandler> logger)
        {
            _next = next;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var claims = _jwtService.ValidateToken(token);

                        // Create a proper ClaimsIdentity with "Bearer" authentication type
                        var identity = new ClaimsIdentity(claims, "Bearer");

                        // Create and set the principal with the authenticated identity
                        context.User = new ClaimsPrincipal(identity);

                        _logger.LogInformation("User authenticated: {Username}",
                            context.User.Identity?.Name ?? "Unknown");
                    }
                    catch (Exception ex)
                    {
                        // Log token validation failure but continue the pipeline
                        _logger.LogWarning("Token validation failed: {Message}", ex.Message);
                    }
                }
            }

            await _next(context);
        }
    }
}
