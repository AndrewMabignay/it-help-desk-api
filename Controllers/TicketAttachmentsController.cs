using HelpDeskSystem.DTOs.TicketAttachments;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:guid}/attachments")]
[Authorize]
public sealed class TicketAttachmentsController : ControllerBase
{
    private readonly ITicketAttachmentService _ticketAttachmentService;

    public TicketAttachmentsController(ITicketAttachmentService ticketAttachmentService)
    {
        _ticketAttachmentService = ticketAttachmentService;
    }

    // GET: api/tickets/{ticketId}/attachments
    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> GetByTicketId(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var attachments = await _ticketAttachmentService.GetByTicketIdAsync(
            ticketId,
            userId,
            cancellationToken);

        return Ok(attachments);
    }

    // POST: api/tickets/{ticketId}/attachments
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        Guid ticketId,
        [FromForm] CreateTicketAttachmentDto dto,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var attachment = await _ticketAttachmentService.UploadAsync(
            ticketId,
            dto,
            userId,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetByTicketId),
            new { ticketId },
            attachment);
    }
}
