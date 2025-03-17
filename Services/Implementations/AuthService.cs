using PizzaStore.Services.Interfaces;
using PizzaStore.DataTransferObjects;
using PizzaStore.Persistence;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;

namespace PizzaStore.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly PizzeriaDbContext _dbContext;
        private readonly IJwtService _jwtService;

        public AuthService(IConfiguration configuration, PizzeriaDbContext dbContext, IJwtService jwtService)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request)
        {
            // Find user by username
            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Username == request.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            // Verify password using bcrypt
            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!validPassword)
                throw new UnauthorizedAccessException("Password and/or username are invalid.");

            // Update last login timestamp
            user.LastLogin = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResponseDto(token);
        }

        public async Task<User> RegisterUserAsync(string username, string email, string password)
        {
            // Check for existing username or email
            if (await _dbContext.Users.AnyAsync(u => u.Username == username))
                throw new InvalidOperationException("Username already exists");

            if (await _dbContext.Users.AnyAsync(u => u.Email == email))
                throw new InvalidOperationException("Email already registered");

            // Generate password hash with bcrypt (automatically includes salt)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Email == email &&
                u.ResetToken == token &&
                u.ResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
                return false;

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return false;

            // Generate token and set expiry (24 hours)
            user.ResetToken = Guid.NewGuid().ToString("N");
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(24);

            await _dbContext.SaveChangesAsync();

            // In a real application, you would send an email with the reset link
            // containing the token
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user.Username, user.Id);
        }
    }
}
