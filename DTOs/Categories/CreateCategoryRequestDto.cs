using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Categories;

public sealed class CreateCategoryRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; init; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; init; }
}
