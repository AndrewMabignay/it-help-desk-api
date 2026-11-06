using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.UserProfiles;

public sealed class UpdateUserProfileRequestDto
{
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
