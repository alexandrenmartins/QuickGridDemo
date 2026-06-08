using System.ComponentModel.DataAnnotations;

namespace QuickGridDemo.Models;

public class Supplier
{
    public int SupplierID { get; set; }

    [Required(ErrorMessage = "Empresa é obrigatória")]
    [StringLength(100, ErrorMessage = "Empresa deve ter no máximo 100 caracteres")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contato é obrigatório")]
    [StringLength(50, ErrorMessage = "Contato deve ter no máximo 50 caracteres")]
    public string ContactName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Cargo deve ter no máximo 50 caracteres")]
    public string? ContactTitle { get; set; }

    [StringLength(100, ErrorMessage = "Endereço deve ter no máximo 100 caracteres")]
    public string? Address { get; set; }

    [StringLength(50, ErrorMessage = "Cidade deve ter no máximo 50 caracteres")]
    public string? City { get; set; }

    [StringLength(50, ErrorMessage = "Região deve ter no máximo 50 caracteres")]
    public string? Region { get; set; }
}
