using Microsoft.EntityFrameworkCore;
using QuickGridDemo.Models;

namespace QuickGridDemo.Data;

public class ApplicationDbContext : DbContext
{
    public string DbPath;
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public ApplicationDbContext()
    {
        var folder = Environment.CurrentDirectory;
        DbPath = System.IO.Path.Join(folder, "Data\\northwind.db");
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Using SQLite for simplicity, but you can use any database provider you like
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

}
