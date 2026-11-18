namespace HelpDeskSystem.DTOs.TicketAttachments;

public sealed class TicketAttachmentDownloadDto
{
    public Stream Content { get; init; } = null!;

    public string FileName { get; init; } = string.Empty;

    public string ContentType { get; init; } = "application/octet-stream";
}
