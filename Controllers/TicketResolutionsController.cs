using HelpDeskSystem.DTOs.TicketResolutions;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[Route("api/tickets/{ticketId:guid}/resolution")]
[ApiController]
[Authorize]
public sealed class TicketResolutionsController : ControllerBase
{
    private readonly ITicketResolutionService _ticketResolutionService;

    public TicketResolutionsController(ITicketResolutionService ticketResolutionService)
    {
        _ticketResolutionService = ticketResolutionService; 
    }

    // POST: api/tickets/{ticketId}/resolution
    [HttpPost]
    [Authorize(Roles = "Admin,ITStaff")]
    public async Task<IActionResult> Create(
        Guid ticketId,
        [FromBody] CreateTicketResolutionDto request,
        CancellationToken cancellationToken)
    {
        var resolvedById = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(resolvedById))
            return Unauthorized();

        var resolution = await _ticketResolutionService.CreateAsync(
            ticketId,
            request,
            resolvedById,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetByTicketId),
            new { ticketId },
            resolution);
    }

    // GET: api/tickets/{ticketId}/resolution
    [HttpGet]
    public async Task<IActionResult> GetByTicketId(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var resolution = await _ticketResolutionService.GetByTicketIdAsync(
            ticketId,
            userId,
            cancellationToken);

        if (resolution is null)
        {
            return NotFound(new
            {
                message = "Ticket resolution not found."
            });
        }

        return Ok(resolution);
    }
}
