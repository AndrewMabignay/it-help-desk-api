using HelpDeskSystem.DTOs.Categories;
using HelpDeskSystem.DTOs.Priorities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/priorities")]
[Authorize]
public sealed class PrioritiesController : ControllerBase
{
    private readonly IPriorityService _priorityService;

    public PrioritiesController(IPriorityService priorityService)
    {
        _priorityService = priorityService;
    }

    // GET: api/priorities
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var priorities = await _priorityService.GetAllAsync(
            pageNumber,
            pageSize,
            searchTerm,
            includeInactive,
            cancellationToken);

        return Ok(priorities);
    }

    // GET: api/priorities/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var priority = await _priorityService.GetByIdAsync(
            id,
            cancellationToken);

        if (priority is null)
        {
            return NotFound(new
            {
                message = "Priority not found."
            });
        }

        return Ok(priority);
    }

    // POST: api/priorities
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreatePriorityRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var priority = await _priorityService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = priority.Id },
                priority);
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

    // PUT: api/priorities/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePriorityRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var priority = await _priorityService.UpdateAsync(
                id,
                request,
                cancellationToken);

            if (priority is null)
            {
                return NotFound(new
                {
                    message = "Priority not found."
                });
            }

            return Ok(priority);
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

    // PATCH: api/priorities/{id}/status
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdatePriorityStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var updated = await _priorityService.UpdateStatusAsync(
            id,
            request,
            cancellationToken);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Priority not found."
            });
        }

        return NoContent();
    }
}
