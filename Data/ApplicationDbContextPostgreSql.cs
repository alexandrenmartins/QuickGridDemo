using Microsoft.EntityFrameworkCore;
using QuickGridDemo.Models;

namespace QuickGridDemo.Data;

public class ApplicationDbContextPostgreSql : ApplicationDbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<EmpresaCliente> EmpresaClientes { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    public ApplicationDbContextPostgreSql(DbContextOptions<ApplicationDbContextPostgreSql> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Customer>();
        modelBuilder.Ignore<Supplier>();

        modelBuilder.Entity<EmpresaCliente>(e =>
        {
            e.HasKey(x => new { x.IdEmpresa, x.IdCliente });
            e.HasOne(x => x.Empresa)
                .WithMany(x => x.EmpresaClientes)
                .HasForeignKey(x => x.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Cliente)
                .WithMany(x => x.EmpresaClientes)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Empresa>()
            .HasMany(x => x.Funcionarios)
            .WithOne(x => x.Empresa)
            .HasForeignKey(x => x.IdEmpresa)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cargo>()
            .HasMany(x => x.Funcionarios)
            .WithOne(x => x.Cargo)
            .HasForeignKey(x => x.IdCargo)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>()
            .HasMany(x => x.Enderecos)
            .WithOne(x => x.Cliente)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>()
            .HasMany(x => x.Pedidos)
            .WithOne(x => x.Cliente)
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Funcionario>()
            .HasMany(x => x.Pedidos)
            .WithOne(x => x.Funcionario)
            .HasForeignKey(x => x.IdFuncionario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pedido>()
            .HasMany(x => x.Itens)
            .WithOne(x => x.Pedido)
            .HasForeignKey(x => x.IdPedido)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Produto>()
            .HasMany(x => x.Itens)
            .WithOne(x => x.Produto)
            .HasForeignKey(x => x.IdProduto)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Categoria>()
            .HasMany(x => x.Produtos)
            .WithOne(x => x.Categoria)
            .HasForeignKey(x => x.IdCategoria)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pedido>()
            .HasOne(x => x.Pagamento)
            .WithOne(x => x.Pedido)
            .HasForeignKey<Pagamento>(x => x.IdPedido)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pagamento>()
            .HasIndex(x => x.IdPedido)
            .IsUnique();
    }
}