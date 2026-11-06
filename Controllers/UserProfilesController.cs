using HelpDeskSystem.DTOs.UserProfiles;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Controllers;

[ApiController]
[Route("api/user-profiles")]
public sealed class UserProfilesController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfilesController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    // GET: api/user-profiles
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var profiles = await _userProfileService.GetAllAsync(cancellationToken);

        return Ok(profiles);
    }

    // GET: api/user-profiles/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var profile = await _userProfileService.GetByIdAsync(
            id,
            cancellationToken);

        if (profile is null)
        {
            return NotFound(new
            {
                message = "User profile not found."
            });
        }

        return Ok(profile);
    }

    // GET: api/user-profiles/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(
        string userId,
        CancellationToken cancellationToken) 
    {
        var profile = await _userProfileService.GetByUserIdAsync(
            userId,
            cancellationToken
        );

        if (profile is null)
        {
            return NotFound(new
            {
                message = "User profile not found."
            });
        }

        return Ok(profile);
    }

    // POST: api/user-profiles
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var profile = await _userProfileService.CreateAsync(
                request,
                cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = profile.Id },
                profile);
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

    // PUT: api/user-profiles/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var profile = await _userProfileService.UpdateAsync(
                id,
                request,
                cancellationToken);

            if (profile is null)
            {
                return NotFound(new
                {
                    message = "User profile not found."
                });
            }

            return Ok(profile);
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
}
