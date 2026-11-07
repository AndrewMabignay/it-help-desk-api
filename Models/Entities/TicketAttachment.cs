namespace HelpDeskSystem.Models.Entities;

public sealed class TicketAttachment
{
    public Guid Id { get; init; }

    // Foreign Keys
    public Guid TicketId { get; init; }

    public string UploadedById { get; init; } = string.Empty;

    // File Information
    public string OriginalFileName { get; init; } = string.Empty;

    public string StoredFileName { get; init; } = string.Empty;

    public string FilePath { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public long FileSizeBytes { get; init; }

    // Navigation Properties
    public Ticket Ticket { get; init; } = null!;

    public ApplicationUser UploadedBy { get; init; } = null!;

    // Timestamp
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
 }
