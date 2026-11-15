using HelpDeskSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Tickets;

public sealed class UpdateTicketStatusRequestDto
{
    [Required]
    [EnumDataType(typeof(TicketStatus))]
    public TicketStatus Status { get; init; }

    [MaxLength(500)]
    public string? Remarks { get; init; }
}
