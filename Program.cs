using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
using PizzaStore.Persistence;
using PizzaStore.Services.Implementations;
using PizzaStore.Services.Interfaces;
using Microsoft.OpenApi.Models;
using PizzaStore.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
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

builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
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

// Map endpoints
app.MapPost("/login", async (AuthRequest request, IAuthService authService) =>
    Results.Ok(await authService.AuthenticateAsync(request)));

app.MapGet("/pizzas/{id}", async (HttpContext context, int id, IPizzaService service, IAuthorizationService authService) => 
{
    if (!authService.IsAuthorized(context))
        return Results.Unauthorized();
        
    return await service.GetByIdAsync(id) is Pizza pizza 
        ? Results.Ok(pizza) 
        : Results.NotFound();
});

app.MapGet("/pizzas", async (IPizzaService service) => 
    await service.GetAllAsync()).WithMetadata(new AuthorizeAttribute());

app.MapPost("/pizzas", async (Pizza pizza, IPizzaService service) => 
    await service.CreateAsync(pizza)).WithMetadata(new AuthorizeAttribute());

app.MapPut("/pizzas", async (Pizza pizza, IPizzaService service) =>
    await service.UpdateAsync(pizza) is Pizza updated
        ? Results.Ok(updated)
        : Results.NotFound()).WithMetadata(new AuthorizeAttribute());

app.MapDelete("/pizzas/{id}", async (int id, IPizzaService service) =>
    await service.DeleteAsync(id)
        ? Results.Ok()
        : Results.NotFound()).WithMetadata(new AuthorizeAttribute());

app.Run();
