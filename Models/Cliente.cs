using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Cliente
{
    [Key]
    [Column("id_cliente")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "Nome do cliente é obrigatório")]
    [StringLength(150)]
    [Column("nome_cliente")]
    public string NomeCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14)]
    [Column("cpf")]
    public string Cpf { get; set; } = string.Empty;

    [StringLength(100)]
    [Column("email")]
    public string? Email { get; set; }

    [StringLength(20)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    public ICollection<EmpresaCliente> EmpresaClientes { get; set; } = new List<EmpresaCliente>();

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
