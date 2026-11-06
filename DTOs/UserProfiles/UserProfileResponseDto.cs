namespace HelpDeskSystem.DTOs.UserProfiles;

public sealed class UserProfileResponseDto
{
    public Guid Id { get; set; }

    // User Information
    public string UserId { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    // Profile Information
    public string? EmployeeNumber { get; init; }

    public string? JobTitle { get; init; }

    public string? PhoneNumber { get; init; }

    public string? ProfilePictureUrl { get; init; }

    // Department Information
    public Guid? DepartmentId { get; init; }

    public string? DepartmentName { get; init; }

    // Timestamps
    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}
