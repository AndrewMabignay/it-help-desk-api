using HelpDeskSystem.Models.Enums;

namespace HelpDeskSystem.DTOs.Tickets;

public sealed class TicketResponseDto
{
    public Guid Id { get; init; }

    public string TicketNumber { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    // Category
    public Guid CategoryId { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    // Priority
    public Guid PriorityId { get; init; }

    public string PriorityName { get; init; } = string.Empty;

    // Requester
    public string RequesterId { get; init; } = string.Empty;

    public string RequesterName { get; init; } = string.Empty;

    // Assigned Support Staff
    public string? AssignedToId { get; init; }

    public string? AssignedToName { get; init; }

    // Status
    public TicketStatus Status { get; init; }

    // Timestamps
    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }

    public DateTime? ResolvedAt { get; init; }

    public DateTime? ClosedAt { get; init; }
}
