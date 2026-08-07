using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Pedido
{
    [Key]
    [Column("id_pedido")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPedido { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("id_funcionario")]
    public int IdFuncionario { get; set; }

    [Column("data_pedido")]
    public DateTime? DataPedido { get; set; }

    [Required(ErrorMessage = "Status do pedido é obrigatório")]
    [StringLength(20)]
    [Column("status_pedido")]
    public string StatusPedido { get; set; } = "Pendente";

    public Cliente? Cliente { get; set; }

    public Funcionario? Funcionario { get; set; }

    public Pagamento? Pagamento { get; set; }

    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
