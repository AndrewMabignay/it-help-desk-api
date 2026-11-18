using HelpDeskSystem.DTOs.TicketAttachments;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:guid}/attachments")]
[Authorize]
public sealed class TicketAttachmentsController : ControllerBase
{
    private readonly ITicketAttachmentService _ticketAttachmentService;

    public TicketAttachmentsController(
        ITicketAttachmentService ticketAttachmentService)
    {
        _ticketAttachmentService = ticketAttachmentService;
    }

    // GET: api/tickets/{ticketId}/attachments
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
            var attachments = await _ticketAttachmentService.GetByTicketIdAsync(
                ticketId,
                userId,
                cancellationToken);

            return Ok(attachments);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    // POST: api/tickets/{ticketId}/attachments
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Upload(
        Guid ticketId,
        [FromForm] CreateTicketAttachmentDto dto,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{attachmentId:guid}/download")]
    public async Task<IActionResult> Download(
        Guid ticketId,
        Guid attachmentId,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        try
        {
            var attachment = await _ticketAttachmentService.DownloadAsync(
                ticketId,
                attachmentId,
                userId,
                cancellationToken);

            return File(
                attachment.Content,
                attachment.ContentType,
                attachment.FileName);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "The attachment file could not be found." });
        }
    }
}
