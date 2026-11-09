namespace HelpDeskSystem.DTOs.TicketWatchers;

public sealed class TicketWatcherResponseDto
{
    public Guid id { get; init; }

    public Guid TicketId { get; init; }

    public string UserId { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}
