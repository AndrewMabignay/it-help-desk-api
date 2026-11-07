using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Models.Entitiesl;

public class TicketResolution
{
    public int Id { get; set; }
    
    public Guid TicketId { get; set; }
    
    public string ResolvedById { get; set; } = string.Empty;

    public string ResolutionSummary { get; set; } = string.Empty;
    
    public string? ResolutionDetails { get; set; }
    
    public DateTime ResolvedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Ticket Ticket { get; set; } = null!;

    public ApplicationUser ResolvedBy { get; set; } = null!;
}
