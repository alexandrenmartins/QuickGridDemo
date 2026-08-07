using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Produto
{
    [Key]
    [Column("id_produto")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdProduto { get; set; }

    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "Nome do produto é obrigatório")]
    [StringLength(150)]
    [Column("nome_produto")]
    public string NomeProduto { get; set; } = string.Empty;

    [Column("preco", TypeName = "numeric(18,2)")]
    public decimal Preco { get; set; }

    [Column("estoque")]
    public int? Estoque { get; set; }

    public Categoria? Categoria { get; set; }

    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
