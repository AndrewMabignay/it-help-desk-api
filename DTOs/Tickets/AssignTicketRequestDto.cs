using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Tickets;

public sealed class AssignTicketRequestDto
{
    [Required]
    public string AssignedToId { get; init; } = string.Empty;
}
