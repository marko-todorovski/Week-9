using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models;

public class Car
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Model must be between 1 and 100 characters.")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Year is required.")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Year must be a 4-digit number.")]
    public string Year { get; set; } = string.Empty;
}
