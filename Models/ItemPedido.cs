using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class ItemPedido
{
    [Key]
    [Column("id_item")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdItem { get; set; }

    [Column("id_pedido")]
    public int IdPedido { get; set; }

    [Column("id_produto")]
    public int IdProduto { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("preco_unitario", TypeName = "numeric(18,2)")]
    public decimal PrecoUnitario { get; set; }

    public Pedido? Pedido { get; set; }

    public Produto? Produto { get; set; }
}
