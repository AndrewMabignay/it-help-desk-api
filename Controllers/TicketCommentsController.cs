using HelpDeskSystem.DTOs.TicketComments;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:guid}/comments")]
[Authorize]
public sealed class TicketCommentsController : ControllerBase
{
    private readonly ITicketCommentService _ticketCommentService;

    public TicketCommentsController(ITicketCommentService ticketCommentService)
    {
        _ticketCommentService = ticketCommentService;
    }

    // GET: api/tickets/{ticketId}/comments
    [HttpGet]
    public async Task<IActionResult> GetByTicketId(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var comments = await _ticketCommentService.GetByTicketIdAsync(
            ticketId,
            userId,
            cancellationToken);

        return Ok(comments);
    }

    // POST: api/tickets/{ticketId}/comments
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid ticketId,
        [FromBody] CreateTicketCommentDto dto,
        CancellationToken cancellationToken)
    {
        if (ticketId != dto.TicketId)
            return BadRequest("Ticket ID mismatch.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var comment = await _ticketCommentService.CreateAsync(
            dto,
            userId,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetByTicketId),
            new { ticketId },
            comment);
    }
}
