using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.TicketResolutions;

public sealed class CreateTicketResolutionDto
{
    [Required]
    [MaxLength(200)]
    public string ResolutionSummary { get; init; } = string.Empty;

    [MaxLength(2000)]
    public string? ResolutionDetails { get; init; }
}
