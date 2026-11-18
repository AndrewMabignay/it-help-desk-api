using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.Notifications;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Models.Enums;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class NotificationService : INotificationService
{
    public readonly ApplicationDbContext _dbContext;

    public NotificationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get user's notifications with pagination
    public async Task<NotificationListResponseDto> GetMyNotificationsAsync(
        string userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _dbContext.Notifications
            .Where(n => n.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);

        var unreadCount = await query
            .CountAsync(n => !n.IsRead, cancellationToken);

        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .ThenByDescending(n => n.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new NotificationListResponseDto
        {
            Notifications = notifications
                .Select(n => n.ToResponseDto())
                .ToList(),

            TotalCount = totalCount,
            UnreadCount = unreadCount
        };
    }

    // Get unread notification count
    public async Task<int> GetUnreadCountAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notifications
            .CountAsync(
                n => n.UserId == userId && !n.IsRead,
                cancellationToken);
    }

    // Mark one notification as read
    public async Task<bool> MarkAsReadAsync(
        Guid notificationId,
        string userId,
        CancellationToken cancellationToken = default)
    {
        var notification = await _dbContext.Notifications
            .FirstOrDefaultAsync(
                n => n.Id == notificationId && n.UserId == userId,
                cancellationToken);

        if (notification is null)
            return false;

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    // Mark all user's notifications as read
    public async Task<int> MarkAllAsReadAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var readAt = DateTime.UtcNow;

        return await _dbContext.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(n => n.IsRead, true)
                    .SetProperty(n => n.ReadAt, readAt),
                cancellationToken);
    }

    // Create notification
    public async Task CreateAsync(
        string userId,
        Guid? ticketId,
        string title,
        string message,
        NotificationType type,
        CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TicketId = ticketId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Notifications.Add(notification);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
