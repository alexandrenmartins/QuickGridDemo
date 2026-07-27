using QuickGridDemo.Components;
using QuickGridDemo.Data;
using Microsoft.EntityFrameworkCore;

//Example from Code Skwela
namespace QuickGridDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddQuickGridEntityFrameworkAdapter();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            // substituir registro atual de DbContextFactory por duas factories especificas
            builder.Services.AddDbContextFactory<ApplicationDbContextSqlite>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

            builder.Services.AddDbContextFactory<ApplicationDbContextSqlServer>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuickGridConnection")));

            // PostgreSQL - Funcionarios
            builder.Services.AddDbContextFactory<ApplicationDbContextPostgreSql>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Auto-migration para SQL Server
            using (var scope = app.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContextSqlServer>>();
                using var context = factory.CreateDbContext();
                context.Database.Migrate();

                // Seed: adicionar 10.000 registros na tabela Customers
                if (!context.Customers.Any())
                {
                    var customers = SeedData.GenerateCustomers(10_000);
                    context.Customers.AddRange(customers);
                    context.SaveChanges();
                }
            }

            // Auto-migration para PostgreSQL
            using (var scope = app.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContextPostgreSql>>();
                using var context = factory.CreateDbContext();
                //context.Database.EnsureCreated(); // Seria para criar o banco e as tabelas, mas não aplica migrações
                context.Database.Migrate();

                // Seed: adicionar 10.000 registros na tabela Employees
                //if (!context.Employees.Any())
                {
                    var employees = SeedData.GenerateEmployees(100_000);
                    context.Employees.AddRange(employees);
                    context.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
    app.UseMigrationsEndPoint();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}