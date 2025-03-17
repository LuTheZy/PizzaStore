using PizzaStore.DataTransferObjects;
using PizzaStore.Models;

namespace PizzaStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
        Task<User> RegisterUserAsync(string username, string email, string password);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> GeneratePasswordResetTokenAsync(string email);
    }
}
