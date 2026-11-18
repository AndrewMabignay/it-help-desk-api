namespace HelpDeskSystem.DTOs.Notifications;

public sealed class NotificationListResponseDto
{
    public IReadOnlyList<NotificationResponseDto> Notifications { get; init; }
        = [];

    public int TotalCount { get; init; }

    public int UnreadCount { get; init; }
}
