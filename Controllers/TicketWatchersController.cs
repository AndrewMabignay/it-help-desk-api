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

        try
        {
            var watchers = await _watcherService.GetByTicketIdAsync(
                ticketId,
                userId,
                cancellationToken);

            return Ok(watchers);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new { message = ex.Message });
        }
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

        try
        {
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
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

        try
        {
            await _watcherService.RemoveAsync(
                ticketId,
                watcherUserId,
                userId,
                cancellationToken);

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new { message = ex.Message });
        }
    }
}
