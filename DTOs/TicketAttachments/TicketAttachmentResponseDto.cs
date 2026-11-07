namespace HelpDeskSystem.DTOs.TicketAttachments;

public sealed class TicketAttachmentResponseDto
{
    public Guid Id { get; init; }

    public Guid TicketId { get; init; }

    public string UploadedById { get; init; } = string.Empty;

    public string UploadedByName { get; init; } = string.Empty;

    public string OriginalFileName { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public long FileSizeBytes { get; init; }

    public DateTime UploadedAt { get; init; }
}
