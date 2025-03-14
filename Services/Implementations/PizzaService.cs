using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
using PizzaStore.Persistence;
using PizzaStore.Services.Interfaces;

namespace PizzaStore.Services.Implementations;

public class PizzaService : IPizzaService
{
    private readonly PizzeriaDbContext _context;

    public PizzaService(PizzeriaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pizza>> GetAllAsync()
        => await _context
            .Pizzas
            .AsNoTracking()
            .ToListAsync();

    public async Task<Pizza?> GetByIdAsync(int id)
        => await _context
            .Pizzas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Pizza> CreateAsync(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();
        return pizza;
    }

    public async Task<Pizza?> UpdateAsync(Pizza pizza)
    {
        var existing = await _context.Pizzas.FindAsync(pizza.Id);
        if (existing == null) return null;

        _context.Entry(existing).CurrentValues.SetValues(pizza);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);
        if (pizza == null) return false;

        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
        return true;
    }
}
