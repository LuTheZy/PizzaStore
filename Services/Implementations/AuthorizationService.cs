using PizzaStore.Services.Interfaces;

namespace PizzaStore.Services.Implementations
{

    public class AuthorizationService : IAuthorizationService
    {
        public bool IsAuthorized(HttpContext context)
        {
            return context.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
