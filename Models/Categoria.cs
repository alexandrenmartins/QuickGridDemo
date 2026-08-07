using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Categoria
{
    [Key]
    [Column("id_categoria")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCategoria { get; set; }

    [Required(ErrorMessage = "Nome da categoria é obrigatório")]
    [StringLength(100)]
    [Column("nome_categoria")]
    public string NomeCategoria { get; set; } = string.Empty;

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
