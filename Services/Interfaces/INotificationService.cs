using HelpDeskSystem.DTOs.Notifications;
using HelpDeskSystem.Models.Enums;

namespace HelpDeskSystem.Services.Interfaces;

public interface INotificationService
{
    Task<NotificationListResponseDto> GetMyNotificationsAsync(
        string userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);

    Task<int> GetUnreadCountAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<bool> MarkAsReadAsync(
        Guid notificationId,
        string userId,
        CancellationToken cancellationToken);

    Task<int> MarkAllAsReadAsync(
        string userId,
        CancellationToken cancellationToken);

    Task CreateAsync(
        string userId,
        Guid? ticketId,
        string title,
        string message,
        NotificationType type,
        CancellationToken cancellationToken);
}
