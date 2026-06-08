using System.ComponentModel.DataAnnotations;

namespace QuickGridDemo.Models;

public class Customer
{
    public string CustomerID { get; set; }

    [Required(ErrorMessage = "Nome do Cliente é obrigatório")]
    public string CompanyName { get; set; }

    [Required(ErrorMessage = "Pessoa de Contato é obrigatória")]
    public string ContactName { get; set; }

    [Required(ErrorMessage = "Função do Contato é obrigatória")]
    public string ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
}