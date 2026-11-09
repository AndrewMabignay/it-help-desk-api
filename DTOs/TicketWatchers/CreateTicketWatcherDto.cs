using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.TicketWatchers;

public sealed class CreateTicketWatcherDto
{
    [Required]
    public string UserId { get; init; } = string.Empty;
}
