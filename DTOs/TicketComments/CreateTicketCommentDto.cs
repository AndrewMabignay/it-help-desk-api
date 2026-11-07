using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.TicketComments;

public sealed class CreateTicketCommentDto
{
    [Required]
    public Guid TicketId { get; init; }

    [Required]
    [StringLength(2000, MinimumLength = 1)]
    public string Comment { get; init; } = string.Empty;

    public bool IsInternal { get; init; } = false;
}
