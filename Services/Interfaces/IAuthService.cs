namespace PizzaStore.Services.Interfaces
{
    public record AuthRequest(string Username, string Password);
    public record AuthResponse(string Token);

    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
    }
}
