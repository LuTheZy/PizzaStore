using PizzaStore.Models;

namespace PizzaStore.Services.Interfaces
{
    public interface IPizzaService
    {
        Task<List<Pizza>> GetAllAsync();
        Task<Pizza?> GetByIdAsync(int id);
        Task<Pizza> CreateAsync(Pizza pizza);
        Task<Pizza?> UpdateAsync(Pizza pizza);
        Task<bool> DeleteAsync(int id);
    }
}