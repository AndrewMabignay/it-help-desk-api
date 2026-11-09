namespace HelpDeskSystem.Models.Entities;

public sealed class TicketWatcher
{
    public Guid Id { get; init; }

    // Foreign Keys
    public Guid TicketId { get; init; }

    public string UserId { get; init; } = string.Empty;

    // Navigation Properties
    public Ticket Ticket { get; init; } = null!;

    public ApplicationUser User { get; init; } = null!;

    // Timestamp
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
