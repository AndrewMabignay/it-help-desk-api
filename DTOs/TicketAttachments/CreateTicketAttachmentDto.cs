namespace HelpDeskSystem.DTOs.TicketAttachments;

public sealed class CreateTicketAttachmentDto
{
    public IFormFile File { get; init; } = null!;
}
