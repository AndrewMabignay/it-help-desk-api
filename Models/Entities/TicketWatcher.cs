namespace HelpDeskSystem.Models.Entities;

public class TicketWatcher
{
    public int Id { get; set; }

    // Foreign Keys
    public int TicketId { get; set; }

    public string UserId { get; set; } = string.Empty;

    // Navigation Properties
    public Ticket Ticket { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
