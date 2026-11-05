using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Users
{
    public sealed class UpdateUserRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; init; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; init; } = string.Empty;

        public int? DepartmentId { get; init; }
    }
}
