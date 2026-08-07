using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Pagamento
{
    [Key]
    [Column("id_pagamento")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPagamento { get; set; }

    [Column("id_pedido")]
    public int IdPedido { get; set; }

    [Column("data_pagamento")]
    public DateTime? DataPagamento { get; set; }

    [Column("valor", TypeName = "numeric(18,2)")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "Método de pagamento é obrigatório")]
    [StringLength(30)]
    [Column("metodo_pagamento")]
    public string MetodoPagamento { get; set; } = string.Empty;

    public Pedido? Pedido { get; set; }
}
