using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Empresa
{
    [Key]
    [Column("id_empresa")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdEmpresa { get; set; }

    [Required(ErrorMessage = "Nome da empresa é obrigatório")]
    [StringLength(150)]
    [Column("nome_empresa")]
    public string NomeEmpresa { get; set; } = string.Empty;

    [Required(ErrorMessage = "CNPJ é obrigatório")]
    [StringLength(18)]
    [Column("cnpj")]
    public string Cnpj { get; set; } = string.Empty;

    [StringLength(200)]
    [Column("endereco")]
    public string? Endereco { get; set; }

    [StringLength(20)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    [StringLength(100)]
    [Column("email")]
    public string? Email { get; set; }

    [Column("data_criacao")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    [Column("data_atualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    [Required(ErrorMessage = "Status é obrigatório")]
    [StringLength(10)]
    [Column("status")]
    public string Status { get; set; } = "Ativo";

    public ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    public ICollection<EmpresaCliente> EmpresaClientes { get; set; } = new List<EmpresaCliente>();
}
