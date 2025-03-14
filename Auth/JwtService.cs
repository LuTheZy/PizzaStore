using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace PizzaStore.Auth;

public interface IJwtService
{
    string GenerateToken(string username);
    IEnumerable<Claim> ValidateToken(string token);
}

public class JwtService : IJwtService
{
    private readonly string _key;
    
    public JwtService(IConfiguration configuration)
    {
        _key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
    }

    public string GenerateToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var header = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"));
        var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(claims)));
        var signature = ComputeSignature(header, payload);

        return $"{header}.{payload}.{signature}";
    }

    public IEnumerable<Claim> ValidateToken(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3)
            throw new SecurityTokenException("Invalid token format");

        var signature = ComputeSignature(parts[0], parts[1]);
        if (signature != parts[2])
            throw new SecurityTokenException("Invalid token signature");

        var payload = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
        var claims = System.Text.Json.JsonSerializer.Deserialize<Claim[]>(payload);
        return claims ?? Array.Empty<Claim>();
    }

    private string ComputeSignature(string header, string payload)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{header}.{payload}"));
        return Convert.ToBase64String(hash);
    }
}
