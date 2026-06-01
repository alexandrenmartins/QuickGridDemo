using System.ComponentModel.DataAnnotations;

namespace QuickGridDemo.Models;
public class Car
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Fabricante é obrigatório")]
    public string Manufacturer { get; set; } = string.Empty;

    [Required(ErrorMessage = "Modelo é obrigatório")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ano é obrigatório")]
    public string Year { get; set; } = string.Empty;
}
