using HelpDeskSystem.DTOs.Notifications;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class NotificationMapping
{
    public static NotificationResponseDto ToResponseDto(
        this Notification notification)
    {
        return new NotificationResponseDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            TicketId = notification.TicketId,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type.ToString(),
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt
        };
    }
}
