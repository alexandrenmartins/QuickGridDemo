using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Cargo
{
    [Key]
    [Column("id_cargo")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCargo { get; set; }

    [Required(ErrorMessage = "Nome do cargo é obrigatório")]
    [StringLength(100)]
    [Column("nome_cargo")]
    public string NomeCargo { get; set; } = string.Empty;

    [StringLength(255)]
    [Column("descricao")]
    public string? Descricao { get; set; }

    [Column("salario_base", TypeName = "numeric(18,2)")]
    public decimal? SalarioBase { get; set; }

    public ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
