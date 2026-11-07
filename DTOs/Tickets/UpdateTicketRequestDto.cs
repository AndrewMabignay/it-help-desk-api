using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Tickets;

public sealed class UpdateTicketRequestDto
{
    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Subject { get; init; } = string.Empty;

    [Required]
    [StringLength(5000, MinimumLength = 10)]
    public string Description { get; init; } = string.Empty;

    [Required]
    public Guid CategoryId { get; init; }

    [Required]
    public Guid PriorityId { get; init; }
}
