
namespace HelpDeskSystem.Models.Entities;

public class TicketAssignment
{
    public int Id { get; set; }

    // Foreign Keys
    public int TicketId { get; set; }

    public string? PreviousAssigneeId { get; set; }

    public string AssignedToId { get; set; } = string.Empty;

    public string AssignedById { get; set; } = string.Empty;

    // Assignment Information
    public string? Remarks { get; set; }

    // Navigation Properties
    public Ticket Ticket { get; set; } = null!;

    public ApplicationUser? PreviousAssignee { get; set; }

    public ApplicationUser AssignedTo { get; set; } = null!;

    public ApplicationUser AssignedBy { get; set; } = null!;

    // Timestamp
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
