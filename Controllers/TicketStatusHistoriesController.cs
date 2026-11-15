using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[Route("api/tickets/{ticketId:guid}/status-history")]
[ApiController]
[Authorize]
public class TicketStatusHistoriesController : ControllerBase
{
    private readonly ITicketStatusHistoryService _historyService;

    public TicketStatusHistoriesController(ITicketStatusHistoryService historyService)
    {
        _historyService = historyService;
    }

    // GET: api/tickets/{ticketId}/status-history
    [HttpGet]
    public async Task<IActionResult> GetByTicketId(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var history = await _historyService.GetByTicketIdAsync(
            ticketId,
            userId,
            cancellationToken);

        return Ok(history);
    }
}
