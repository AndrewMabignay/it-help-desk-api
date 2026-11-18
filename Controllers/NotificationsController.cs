using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }


    // GET: api/notifications?page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notifications = await _notificationService.GetMyNotificationsAsync(
            userId,
            page,
            pageSize,
            cancellationToken);

        return Ok(notifications);
    }

    // GET: api/notifications/unread-count
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var count = await _notificationService.GetUnreadCountAsync(
            userId,
            cancellationToken);

        return Ok(new { unreadCount = count });
    }

    // PATCH: api/notifications/{notificationId}/read
    [HttpPatch("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var success = await _notificationService.MarkAsReadAsync(
            notificationId,
            userId,
            cancellationToken);

        if (!success)
            return NotFound(new { message = "Notification not found." });

        return NoContent();
    }

    // PATCH: api/notifications/read-all
    [HttpPatch("read-all")]
    public async Task<IActionResult> MarkAllAsRead(
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var updatedCount = await _notificationService.MarkAllAsReadAsync(
            userId,
            cancellationToken);

        return Ok(new { updatedCount });
    }
}
