using HelpDeskSystem.Models;
using HelpDeskSystem.Models.Enums;

namespace HelpDeskSystem.Models.Entities;

public class Ticket
{
    public int Id { get; set; }

    // Ticket Information
    public string TicketNumber { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Foreign Keys
    public Guid CategoryId { get; set; }

    public int PriorityId { get; set; }

    public string RequesterId { get; set; } = string.Empty;

    public string? AssignedToId { get; set; }

    // Ticket Status
    public TicketStatus Status { get; set; } = TicketStatus.Open;

    // Navigation Properties
    public Category Category { get; set; } = null!;

    public Priority Priority { get; set; } = null!;

    public ApplicationUser Requester { get; set; } = null!;

    public ApplicationUser? AssignedTo { get; set; }

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? ClosedAt { get; set; }
}
