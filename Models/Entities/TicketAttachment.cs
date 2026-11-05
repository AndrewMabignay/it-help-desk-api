namespace HelpDeskSystem.Models.Entities;

public class TicketAttachment
{
    public int Id { get; set; }

    // Foreign Keys
    public int TicketId { get; set; }

    public string UploadedById { get; set; } = string.Empty;

    // File Information
    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    // Navigation Properties
    public Ticket Ticket { get; set; } = null!;

    public ApplicationUser UploadedBy { get; set; } = null!;

    // Timestamp
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
 }
