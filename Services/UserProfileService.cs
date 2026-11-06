using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.UserProfiles;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class UserProfileService : IUserProfileService
{
    private readonly ApplicationDbContext _dbContext;

    public UserProfileService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all user profiles
    public async Task<IReadOnlyList<UserProfileResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
            .AsNoTracking()
            .Include(up => up.User)
            .Include(up => up.Department)
            .Select(up => new UserProfileResponseDto
            {
                Id = up.Id,
                UserId = up.UserId,
                FirstName = up.User.FirstName,
                LastName = up.User.LastName,
                Email = up.User.Email!,
                EmployeeNumber = up.EmployeeNumber,
                JobTitle = up.JobTitle,
                PhoneNumber = up.PhoneNumber,
                ProfilePictureUrl = up.ProfilePictureUrl,
                DepartmentId = up.DepartmentId,
                DepartmentName = up.Department != null
                    ? up.Department.Name
                    : null,
                CreatedAt = up.CreatedAt,
                UpdatedAt = up.UpdatedAt
            })
            .ToListAsync();
    }

    // Get user profile by ID
    public async Task<UserProfileResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
            .AsNoTracking()
            .Where(up => up.Id == id)
            .Select(up => new UserProfileResponseDto
            {
                Id = up.Id,
                UserId = up.UserId,
                FirstName = up.User.FirstName,
                LastName = up.User.LastName,
                Email = up.User.Email!,
                EmployeeNumber = up.EmployeeNumber,
                JobTitle = up.JobTitle,
                PhoneNumber = up.PhoneNumber,
                ProfilePictureUrl = up.ProfilePictureUrl,
                DepartmentId = up.DepartmentId,
                DepartmentName = up.Department != null
                ? up.Department.Name
                : null,
                CreatedAt = up.CreatedAt,
                UpdatedAt = up.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    // Get user profile by User ID
    public async Task<UserProfileResponseDto?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.UserProfiles
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .Select(up => new UserProfileResponseDto
            {
                Id = up.Id,
                UserId = up.UserId,
                FirstName = up.User.FirstName,
                LastName = up.User.LastName,
                Email = up.User.Email!,
                EmployeeNumber = up.EmployeeNumber,
                JobTitle = up.JobTitle,
                PhoneNumber = up.PhoneNumber,
                ProfilePictureUrl = up.ProfilePictureUrl,
                DepartmentId = up.DepartmentId,
                DepartmentName = up.Department != null
                    ? up.Department.Name
                    : null,
                CreatedAt = up.CreatedAt,
                UpdatedAt = up.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    // Create user profile
    public async Task<UserProfileResponseDto> CreateAsync(
        CreateUserProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        // Check if the Identity user exists.
        var userExists = await _dbContext.Users
            .AnyAsync(
                u => u.Id == request.UserId,
                cancellationToken
            );

        if (!userExists) throw new KeyNotFoundException("User not found.");

        // Check for an existing profile.
        var existingUserProfile = await _dbContext.UserProfiles
            .AnyAsync(
                up => up.UserId == request.UserId,
                cancellationToken
            );

        if (existingUserProfile) throw new InvalidOperationException("This user already has a profile.");

        // Validate DepartmentId if supplied.
        if (request.DepartmentId.HasValue)
        {
            var departmentExists = await _dbContext.Departments
                .AnyAsync(
                    d => d.Id == request.DepartmentId.Value,
                    cancellationToken
                );

            if (!departmentExists) throw new KeyNotFoundException("Department not found.");
        }

        var profile = new UserProfile
        {
            UserId = request.UserId,
            EmployeeNumber = request.EmployeeNumber,
            JobTitle = request.JobTitle,
            PhoneNumber = request.PhoneNumber,
            ProfilePictureUrl = request.ProfilePictureUrl,
            DepartmentId = request.DepartmentId
        };

        _dbContext.UserProfiles.Add(profile);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var createdProfile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Include(up => up.User)
            .Include(up => up.Department)
            .FirstAsync(
                up => up.Id == profile.Id,
                cancellationToken
            );

        return MapToResponse(createdProfile);
    }

    // Update user profile
    public async Task<UserProfileResponseDto?> UpdateAsync(
        Guid id,
        UpdateUserProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        var profile = await _dbContext.UserProfiles
            .FirstOrDefaultAsync(
                up => up.Id == id,
                cancellationToken
            );

        if (profile is null) return null;

        // Validate DepartmentId if supplied.
        if (request.DepartmentId.HasValue)
        {
            var departmentExists = await _dbContext.Departments
                .AnyAsync(
                    d => d.Id == request.DepartmentId.Value,
                    cancellationToken
                );

            if (!departmentExists) throw new KeyNotFoundException("Department not found.");
        }

        profile.EmployeeNumber = request.EmployeeNumber;
        profile.JobTitle = request.JobTitle;
        profile.PhoneNumber = request.PhoneNumber;
        profile.ProfilePictureUrl = request.ProfilePictureUrl;
        profile.DepartmentId = request.DepartmentId;
        profile.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var createdProfile = await _dbContext.UserProfiles
            .AsNoTracking()
            .Include(up => up.User)
            .Include(up => up.Department)
            .FirstAsync(
                up => up.Id == profile.Id,
                cancellationToken
            );

        return MapToResponse(createdProfile);
    }

    // Map entity to response DTO
    private static UserProfileResponseDto MapToResponse(UserProfile userProfile)
    {
        return new UserProfileResponseDto
        {
            Id = userProfile.Id,
            UserId = userProfile.UserId,
            FirstName = userProfile.User.FirstName,
            LastName = userProfile.User.LastName,
            Email = userProfile.User.Email!,
            EmployeeNumber = userProfile.EmployeeNumber,
            JobTitle = userProfile.JobTitle,
            PhoneNumber = userProfile.PhoneNumber,
            ProfilePictureUrl = userProfile.ProfilePictureUrl,
            DepartmentId = userProfile.DepartmentId,
            DepartmentName = userProfile.Department?.Name,
            CreatedAt = userProfile.CreatedAt,
            UpdatedAt = userProfile.UpdatedAt
        };
    }
}
