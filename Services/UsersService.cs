using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.Users;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _dbContext = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<PagedResponseDto<UserResponseDto>> GetUsersAsynch(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken) 
    {
        var query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(user =>
                user.Email!.Contains(searchTerm) ||
                user.FirstName.Contains(searchTerm) ||
                user.LastName.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .OrderBy(user => user.LastName)
            .ThenBy(user => user.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var userResponses = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userResponses.Add(new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DepartmentId = null,
                Roles = roles,
                IsActive = user.IsActive
            });
        }

        return new PagedResponseDto<UserResponseDto>
        {
            Items = userResponses,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<UserResponseDto> GetUserByIdAsync(
        string id,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DepartmentId = null,
            Roles = roles,
            IsActive = user.IsActive
        };
    }

    public async Task<UserResponseDto> CreateAsynch(
        CreateUserRequestDto request,
        CancellationToken cancellationToken) 
    {
        // Check if email already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        // Validate role
        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
        {
            throw new InvalidOperationException("Invalid role.");
        }

        // Create user
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user: {errors}");
        }

        // Assign role
        var roleResult = await _userManager.AddToRoleAsync(user, request.Role);

        if (!roleResult.Succeeded)
        {
            // Roll back user creation if role assignment fails
            await _userManager.DeleteAsync(user);

            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to assign role: {errors}");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DepartmentId = null,
            Roles = roles,
            IsActive = user.IsActive
        };
    }

    public async Task<UserResponseDto?> UpdateAsync(
        string id,
        UpdateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        // Find existing user
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        // Check if email is already used by another user
        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null && existingUser.Id != user.Id)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        // Update user information
        user.Email = request.Email;
        user.UserName = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        // Get updated roles
        var roles = await _userManager.GetRolesAsync(user);

        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DepartmentId = null,
            Roles = roles,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> UpdateStatusAsync(
    string id,
    bool isActive,
    CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return false;
        }

        user.IsActive = isActive;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        return true;
    }
}
