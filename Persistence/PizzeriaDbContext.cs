using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;
using PizzaStore.Persistence.Configurations;

namespace PizzaStore.Persistence
{
    public class PizzeriaDbContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }

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
        }
    }
}