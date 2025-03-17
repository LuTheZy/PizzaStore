using PizzaStore.DataTransferObjects;

namespace PizzaStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto request);
    }
}
