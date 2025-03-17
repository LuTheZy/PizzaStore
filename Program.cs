using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
using PizzaStore.Persistence;
using PizzaStore.Services.Implementations;
using PizzaStore.Services.Interfaces;
using Microsoft.OpenApi.Models;
using PizzaStore.Auth;
using PizzaStore.Middleware;
using PizzaStore.Attributes;
using PizzaStore.DataTransferObjects;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzeriaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PizzeriaDbConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStore API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddScoped<IPizzaService, PizzaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API v1"));
}

// Use custom JWT authentication and authorization
app.UseMiddleware<JwtAuthenticationHandler>();
app.UseMiddleware<AuthorizationMiddleware>();

// Authentication endpoints
app.MapPost("/login", async (AuthRequestDto request, IAuthService authService) =>
    Results.Ok(await authService.AuthenticateAsync(request)));

// New user registration endpoint
app.MapPost("/register", async (RegisterUserDto request, IAuthService authService) =>
{
    try
    {
        var user = await authService.RegisterUserAsync(
            request.Username,
            request.Email,
            request.Password
        );

        return Results.Created($"/users/{user.Id}", new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.StatusCode(500);
    }
});

// Password reset request endpoint
app.MapPost("/forgot-password", async (ForgotPasswordDto request, IAuthService authService) =>
{
    bool success = await authService.GeneratePasswordResetTokenAsync(request.Email);

    // Always return OK even if email not found for security reasons
    return Results.Ok(new { message = "If your email is registered, you will receive a password reset link." });
});

// Password reset confirmation endpoint
app.MapPost("/reset-password", async (ResetPasswordDto request, IAuthService authService) =>
{
    bool success = await authService.ResetPasswordAsync(
        request.Email,
        request.Token,
        request.NewPassword
    );

    if (success)
        return Results.Ok(new { message = "Password has been reset successfully." });
    else
        return Results.BadRequest(new { error = "Invalid or expired password reset token." });
});

// Pizza endpoints with authorization
app.MapGet("/pizzas/{id}", async (int id, IPizzaService service) =>
    await service.GetByIdAsync(id) is Pizza pizza
        ? Results.Ok(pizza)
        : Results.NotFound()).WithMetadata(new AuthorizeAttribute());

app.MapGet("/pizzas", async (IPizzaService service) =>
    Results.Ok(await service.GetAllAsync())).WithMetadata(new AuthorizeAttribute());

app.MapPost("/pizzas", async (Pizza pizza, IPizzaService service) =>
    Results.Ok(await service.CreateAsync(pizza))).WithMetadata(new AuthorizeAttribute());

app.MapPut("/pizzas", async (Pizza pizza, IPizzaService service) =>
    await service.UpdateAsync(pizza) is Pizza updated
        ? Results.Ok(updated)
        : Results.NotFound()).WithMetadata(new AuthorizeAttribute());

app.MapDelete("/pizzas/{id}", async (int id, IPizzaService service) =>
    await service.DeleteAsync(id)
        ? Results.Ok()
        : Results.NotFound()).WithMetadata(new AuthorizeAttribute());

app.Run();
