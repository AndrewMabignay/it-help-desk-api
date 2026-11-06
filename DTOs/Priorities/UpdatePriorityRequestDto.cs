using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Priorities;

public sealed class UpdatePriorityRequestDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }

    [Range(1, 5)]
    public int Level { get; set; }
}
