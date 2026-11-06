using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.UserProfiles;

public sealed class CreateUserProfileRequestDto
{
    [Required]
    public string UserId { get; init; } = string.Empty;

    [MaxLength(50)]
    public string? EmployeeNumber { get; init; }

    [MaxLength(100)]
    public string? JobTitle { get; init; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; init; }

    [Url]
    [MaxLength(500)]
    public string? ProfilePictureUrl { get; init; }

    public Guid? DepartmentId { get; init; }
}