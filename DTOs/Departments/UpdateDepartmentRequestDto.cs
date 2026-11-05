using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Departments;

public sealed class UpdateDepartmentRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; init; }
}   
