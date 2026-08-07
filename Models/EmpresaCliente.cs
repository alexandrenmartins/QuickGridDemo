using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class EmpresaCliente
{
    [Key]
    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Key]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    public Empresa? Empresa { get; set; }

    public Cliente? Cliente { get; set; }
}
