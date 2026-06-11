using Microsoft.EntityFrameworkCore;

namespace QuickGridDemo.Data;

public class ApplicationDbContextSqlite : ApplicationDbContext
{
    public ApplicationDbContextSqlite(DbContextOptions<ApplicationDbContextSqlite> options)
        : base(options)
    {
    }
}