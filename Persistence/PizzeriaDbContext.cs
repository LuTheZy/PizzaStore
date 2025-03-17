using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
using PizzaStore.Persistence.Configurations;

namespace PizzaStore.Persistence
{
    public class PizzeriaDbContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<User> Users { get; set; }

        public PizzeriaDbContext(DbContextOptions<PizzeriaDbContext> options)
            : base(options)
        {
        }

        public PizzeriaDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PizzaConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());

        }
    }
}