using System.Security.Claims;

namespace PizzaStore.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username, int userId);
        IEnumerable<Claim> ValidateToken(string token);
    }
}
