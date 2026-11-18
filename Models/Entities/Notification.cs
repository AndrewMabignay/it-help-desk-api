using HelpDeskSystem.Models.Enums;

namespace HelpDeskSystem.Models.Entities;

public sealed class Notification
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public Guid? TicketId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public NotificationType Type { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAt { get; set; }

    // Navigation Properties
    public ApplicationUser User { get; set; } = null!;

    public Ticket? Ticket { get; set; }
}
