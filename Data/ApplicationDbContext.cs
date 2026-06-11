using Microsoft.EntityFrameworkCore;
using QuickGridDemo.Models;

namespace QuickGridDemo.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
}
