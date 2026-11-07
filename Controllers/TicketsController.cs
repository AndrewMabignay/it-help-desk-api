using HelpDeskSystem.DTOs.Tickets;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public sealed class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // GET: api/tickets
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] TicketQueryDto query,
        CancellationToken cancellationToken) 
    {
        var tickets = await _ticketService.GetAllAsync(
            query,
            cancellationToken);

        return Ok(tickets);
    }

    // GET: api/tickets/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.GetByIdAsync(
            id,
            cancellationToken);

        if (ticket is null)
        {
            return NotFound(new 
            {
                message = "Ticket not found."
            });
        }

        return Ok(ticket);
    }

    // POST: api/tickets
    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketRequestDto request,
        CancellationToken cancellationToken) 
    {
        try
        {
            var requesterId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(requesterId))
            {
                return Unauthorized();
            }

            var ticket = await _ticketService.CreateAsync(
                request, 
                requesterId, 
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = ticket.Id },
                ticket
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/tickets/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTicketRequestDto request,
        CancellationToken cancellationToken)
    {
        try 
        {
            var ticket = await _ticketService.UpdateAsync(
               id,
               request,
               cancellationToken);

            if (ticket == null)
            {
                return NotFound(new
                {
                    message = "Ticket not found."
                });
            }

            return Ok(ticket);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/tickets/{id}/assign
    [HttpPut("{id:guid}/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Assign(
        Guid id,
        [FromBody] AssignTicketRequestDto request,
        CancellationToken cancellationToken)
    {
        try 
        {
            var result = await _ticketService.AssignAsync(
                id,
                request,
                cancellationToken);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Ticket not found."
                });
            }

            return Ok(new
            {
                message = "Ticket successfully assigned."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PATCH: api/tickets/{id}/status
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,ITStaff")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateTicketStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _ticketService.UpdateStatusAsync(
            id, 
            request,
            cancellationToken);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Ticket not found."
            });
        }

        return Ok(new
        {
            message = "Ticket status successfully updated."
        });
    }
}
