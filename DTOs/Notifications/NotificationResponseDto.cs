namespace HelpDeskSystem.DTOs.Notifications;

public sealed class NotificationResponseDto
{
    public Guid Id { get; init; }

    public string UserId { get; init; } = string.Empty;

    public Guid? TicketId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string Type { get; init; } = string.Empty;

    public bool IsRead { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? ReadAt { get; init; }
}
