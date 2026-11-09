using HelpDeskSystem.DTOs.TicketWatchers;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:guid}/watchers")]
[Authorize]
public sealed class TicketWatchersController : ControllerBase
{
    private readonly ITicketWatcherService _watcherService;

    public TicketWatchersController(ITicketWatcherService watcherService)
    {
        _watcherService = watcherService;
    }

    // GET: api/tickets/{ticketId}/watchers
    [HttpGet]
    public async Task<IActionResult> GetByTicketId(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var watchers = await _watcherService.GetByTicketIdAsync(
            ticketId,
            userId,
            cancellationToken);

        return Ok(watchers);
    }

    // POST: api/tickets/{ticketId}/watchers
    [HttpPost]
    public async Task<IActionResult> Add(
        Guid ticketId,
        [FromBody] CreateTicketWatcherDto dto,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var watcher = await _watcherService.AddAsync(
            ticketId,
            dto,
            userId,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetByTicketId),
            new { ticketId },
            watcher);
    }

    // DELETE: api/tickets/{ticketId}/watchers/{watcherUserId}
    [HttpDelete("{watcherUserId}")]
    public async Task<IActionResult> Remove(
        Guid ticketId,
        string watcherUserId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _watcherService.RemoveAsync(
            ticketId,
            watcherUserId,
            userId,
            cancellationToken);

        return NoContent();
    }
}
