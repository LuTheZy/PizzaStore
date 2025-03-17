namespace PizzaStore.Services.Interfaces
{
    public interface IAuthorizationService
    {
        bool IsAuthorized(HttpContext context);
    }
}
