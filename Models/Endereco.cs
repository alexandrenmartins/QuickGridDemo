using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Endereco
{
    [Key]
    [Column("id_endereco")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdEndereco { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [StringLength(150)]
    [Column("logradouro")]
    public string? Logradouro { get; set; }

    [StringLength(20)]
    [Column("numero")]
    public string? Numero { get; set; }

    [StringLength(100)]
    [Column("complemento")]
    public string? Complemento { get; set; }

    [StringLength(100)]
    [Column("bairro")]
    public string? Bairro { get; set; }

    [StringLength(100)]
    [Column("cidade")]
    public string? Cidade { get; set; }

    [StringLength(2)]
    [Column("estado")]
    public string? Estado { get; set; }

    [StringLength(10)]
    [Column("cep")]
    public string? Cep { get; set; }

    [StringLength(50)]
    [Column("pais")]
    public string? Pais { get; set; }

    [StringLength(20)]
    [Column("tipo_endereco")]
    public string? TipoEndereco { get; set; }

    [Column("data_criacao")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public Cliente? Cliente { get; set; }
}
