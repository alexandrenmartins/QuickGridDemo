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

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddQuickGridEntityFrameworkAdapter();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            // substituir registro atual de DbContextFactory por duas factories específicas
            builder.Services.AddDbContextFactory<ApplicationDbContextSqlite>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

            builder.Services.AddDbContextFactory<ApplicationDbContextSqlServer>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuickGridConnection")));
            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Auto-migration para SQL Server
            using (var scope = app.Services.CreateScope())
            {
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContextSqlServer>>();
                using var context = factory.CreateDbContext();
                context.Database.Migrate();

                // Seed: popular Customers com 10.000 registros se a tabela estiver vazia
                if (!context.Customers.Any())
                {
                    var customers = SeedData.GenerateCustomers(10_000);
                    context.Customers.AddRange(customers);
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
