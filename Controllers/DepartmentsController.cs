using HelpDeskSystem.DTOs.Departments;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/departments")]
[Authorize]
public sealed class DepartmentsController : ControllerBase
{
    private readonly IDepartmentsService _departmentsService;

    public DepartmentsController(IDepartmentsService departmentsService)
    { 
        _departmentsService = departmentsService;
    }

    // GET: api/departments
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        var departments = await _departmentsService.GetAllAsync(
            includeInactive,
            cancellationToken);

        return Ok(departments);
    }

    // GET: api/departments/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var department = await _departmentsService.GetByIdAsync(
            id,
            cancellationToken);

        if (department is null)
        {
            return NotFound(new
            { 
                message = "Department not found."
            });
        }

        return Ok(department);
    }

    // POST: api/departments
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var department = await _departmentsService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id },
                department);
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

    // PUT: api/departments/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDepartmentRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var department = await _departmentsService.UpdateAsync(
                id,
                request,
                cancellationToken);

            if (department is null)
            {
                return NotFound(new
                {
                    message = "Department not found."
                });
            }

            return Ok(department);
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

    // PATCH: api/departments/{id}/status
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateDepartmentStatusRequestDto request,
        CancellationToken cancellationToken) 
    {
        var updated = await _departmentsService.UpdateStatusAsync(
            id,
            request,
            cancellationToken);

        if (!updated)
        {
            return NotFound(new 
            { 
                message = "Department not found."
            });
        }

        return NoContent();
    }
}
