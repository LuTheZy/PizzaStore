using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using PizzaStore.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace PizzaStore.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
            _issuer = configuration["Jwt:Issuer"] ?? "PizzaStore";
            _audience = configuration["Jwt:Audience"] ?? "PizzaStoreClient";
        }

        public string GenerateToken(string username, int userId)
        {
            var claimsList = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Iss, _issuer),
                    new Claim(JwtRegisteredClaimNames.Aud, _audience),
                    new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds().ToString())
                };

            // Create a ClaimsDictionary to serialize
            var claimsDictionary = claimsList.ToDictionary(
                c => c.Type,
                c => c.Value
            );

            var header = Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"alg\":\"HS256\",\"typ\":\"JWT\"}"))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(claimsDictionary)))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var signature = ComputeSignature(header, payload)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            return $"{header}.{payload}.{signature}";
        }

        public IEnumerable<Claim> ValidateToken(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                throw new SecurityTokenException("Invalid token format");

            var headerSegment = parts[0];
            var payloadSegment = parts[1];
            var signatureSegment = parts[2];

            // Base64Url decode
            string Decode(string input)
            {
                input = input.Replace('-', '+').Replace('_', '/');
                switch (input.Length % 4)
                {
                    case 0: break;
                    case 2: input += "=="; break;
                    case 3: input += "="; break;
                    default: throw new SecurityTokenException("Invalid base64url string");
                }
                return input;
            }

            var computedSignature = ComputeSignature(headerSegment, payloadSegment)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            if (computedSignature != signatureSegment)
                throw new SecurityTokenException("Invalid token signature");

            var payload = Encoding.UTF8.GetString(Convert.FromBase64String(Decode(payloadSegment)));

            // Deserialize to dictionary
            var claimsDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(payload);

            if (claimsDictionary == null)
                return Array.Empty<Claim>();

            // Validate expiration
            if (claimsDictionary.TryGetValue(JwtRegisteredClaimNames.Exp, out var expValue) &&
                long.TryParse(expValue, out var expUnixTime))
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTime);
                if (expirationTime < DateTimeOffset.UtcNow)
                    throw new SecurityTokenException("Token has expired");
            }

            // Convert dictionary back to Claims
            return claimsDictionary.Select(kvp => new Claim(kvp.Key, kvp.Value));
        }

        private string ComputeSignature(string header, string payload)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{header}.{payload}"));
            return Convert.ToBase64String(hash);
        }
    }
}