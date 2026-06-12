using Microsoft.EntityFrameworkCore;
using QuickGridDemo.Models;

namespace QuickGridDemo.Data;

public class ApplicationDbContextPostgreSql : ApplicationDbContext
{
    public DbSet<Employee> Employees { get; set; }

    public ApplicationDbContextPostgreSql(DbContextOptions<ApplicationDbContextPostgreSql> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Customer>();
        modelBuilder.Ignore<Supplier>();
    }
}