using HelpDeskSystem.Models.Enums;

namespace HelpDeskSystem.Models.Entities;

public class TicketStatusHistory
{
    public int Id { get; set; }

    // Foreign Keys
    public Guid TicketId { get; set; }

    public string? ChangedById { get; set; }
    
    // Status Information
    public TicketStatus? OldStatus { get; set; }
    
    public TicketStatus NewStatus { get; set; }

    public string? Remarks { get; set; }

    // Navigation Properties
    public Ticket Ticket { get; set; } = null!;

    public ApplicationUser? ChangedBy { get; set; }

    // Timestamp
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
