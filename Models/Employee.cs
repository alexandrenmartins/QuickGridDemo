using System.ComponentModel.DataAnnotations;

namespace QuickGridDemo.Models;

public class Employee
{
    public int EmployeeID { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sobrenome é obrigatório")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(60)]
    public string? Title { get; set; }

    [StringLength(25)]
    public string? TitleOfCourtesy { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? HireDate { get; set; }

    [StringLength(100)]
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
    public string? HomePhone { get; set; }

    [StringLength(10)]
    public string? Extension { get; set; }

    public string? Notes { get; set; }
}
