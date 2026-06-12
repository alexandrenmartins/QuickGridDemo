using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickGridDemo.Models;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerID { get; set; }

    [Required(ErrorMessage = "Nome do Cliente é obrigatório")]
    [StringLength(40)]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pessoa de Contato é obrigatória")]
    [StringLength(30)]
    public string ContactName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Função do Contato é obrigatória")]
    [StringLength(50)]
    public string ContactTitle { get; set; } = string.Empty;

    [StringLength(60)]
    public string? Address { get; set; }

    [StringLength(40)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? Region { get; set; }

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(40)]
    public string? Country { get; set; }

    [StringLength(24)]
    public string? Phone { get; set; }

    [StringLength(24)]
    public string? Fax { get; set; }
}