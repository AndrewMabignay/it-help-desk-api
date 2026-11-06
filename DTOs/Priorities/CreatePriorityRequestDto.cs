using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Priorities;

public sealed class CreatePriorityRequestDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; init; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; init; }

    [Range(1, 5)]
    public int Level { get; init; }
}
