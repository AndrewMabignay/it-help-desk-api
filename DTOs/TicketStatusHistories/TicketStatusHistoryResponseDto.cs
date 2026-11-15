namespace HelpDeskSystem.DTOs.TicketStatusHistories;

public sealed class TicketStatusHistoryResponseDto
{
    public Guid Id { get; init; }

    public Guid TicketId { get; init; }

    public string? ChangedById { get; init; }

    public string? ChangedByName { get; init; }

    public string? OldStatus { get; init; }

    public string NewStatus { get; init; } = string.Empty;

    public string? Remarks { get; init; }

    public DateTime ChangedAt { get; init; }
}
