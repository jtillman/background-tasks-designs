using DefinedCaller.Models;
using Microsoft.EntityFrameworkCore;

namespace DefinedCaller
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext() { }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}
