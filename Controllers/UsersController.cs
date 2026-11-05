using HelpDeskSystem.DTOs.Users;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public sealed class UsersController : Controller
{
    private readonly IUsersService _userService;

    public UsersController(IUsersService userService)
    {
        _userService = userService;
    }

    // GET: api/users?pageNumber=1&pageSize=10&search=John
    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResponseDto<UserResponseDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponseDto<UserResponseDto>>> GetUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) 
        {
            ModelState.AddModelError(
                nameof(pageNumber),
                "Page number must be greater than zero.");

            return ValidationProblem(ModelState);
        }

        if (pageSize < 1 || pageSize > 100)
        {
            ModelState.AddModelError(
                nameof(pageSize),
                "Page size must be between 1 and 100.");

            return ValidationProblem(ModelState);
        }

        if (search?.Length > 100)
        {
            ModelState.AddModelError(
                nameof(search),
                "Search cannot exceed 100 characters.");

            return ValidationProblem(ModelState);
        }

        var result = await _userService.GetUsersAsynch(
            pageNumber,
            pageSize,
            search,
            cancellationToken);

        return Ok(result);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(
        typeof(UserResponseDto), 
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponseDto>> GetUser(
        string id, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.GetUserByIdAsync(
            id, 
            cancellationToken);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    [ProducesResponseType(
        typeof(UserResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> CreateUser(
        [FromBody] CreateUserRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.CreateAsynch(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(
        typeof(UserResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> UpdateUser(
        string id,
        [FromBody] UpdateUserRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userService.UpdateAsync(
            id,
            request,
            cancellationToken);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    // PATCH: api/users/{id}/status
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<UserResponseDto>> UpdateUserStatus(
        string id,
        [FromBody] UpdateUserStatusDto request,
        CancellationToken cancellationToken = default)
    {
        var updated = await _userService.UpdateStatusAsync(
            id,
            request.IsActive,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}
