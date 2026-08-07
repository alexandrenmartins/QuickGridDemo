using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Funcionario
{
    [Key]
    [Column("id_funcionario")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdFuncionario { get; set; }

    [Column("id_cargo")]
    public int IdCargo { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Required(ErrorMessage = "Nome do funcionário é obrigatório")]
    [StringLength(150)]
    [Column("nome_funcionario")]
    public string NomeFuncionario { get; set; } = string.Empty;

    [Column("data_admissao")]
    public DateTime? DataAdmissao { get; set; }

    [Column("salario", TypeName = "numeric(18,2)")]
    public decimal? Salario { get; set; }

    public Empresa? Empresa { get; set; }

    public Cargo? Cargo { get; set; }

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
