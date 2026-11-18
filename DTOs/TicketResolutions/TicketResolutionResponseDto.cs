namespace HelpDeskSystem.DTOs.TicketResolutions;

public sealed class TicketResolutionResponseDto
{
    public Guid id { get; init; }

    public Guid TicketId { get; init; }

    public string ResolvedById { get; init; } = string.Empty;

    public string ResolvedByName { get; init; } = string.Empty;

    public string ResolutionSummary { get; init; } = string.Empty;

    public string? ResolutionDetails { get; init; }

    public DateTime ResolvedAt { get; init; }
}
