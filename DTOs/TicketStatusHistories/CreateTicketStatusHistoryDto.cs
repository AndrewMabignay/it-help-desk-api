using HelpDeskSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.TicketStatusHistories;

public sealed class CreateTicketStatusHistoryDto
{
    public TicketStatus? OldStatus { get; init; }

    [Required]
    public TicketStatus NewStatus { get; init; }

    [MaxLength(500)]
    public string? Remarks { get; init; }
}
