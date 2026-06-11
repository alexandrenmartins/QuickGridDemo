using Microsoft.EntityFrameworkCore;

namespace QuickGridDemo.Data;

public class ApplicationDbContextSqlServer : ApplicationDbContext
{
    public ApplicationDbContextSqlServer(DbContextOptions<ApplicationDbContextSqlServer> options)
        : base(options)
    {
    }
}