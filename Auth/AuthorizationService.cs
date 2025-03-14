namespace PizzaStore.Auth;

public interface IAuthorizationService
{
    bool IsAuthorized(HttpContext context);
}

public class AuthorizationService : IAuthorizationService
{
    public bool IsAuthorized(HttpContext context)
    {
        return context.User.Identity?.IsAuthenticated ?? false;
    }
}
