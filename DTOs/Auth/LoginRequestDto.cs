using System.ComponentModel.DataAnnotations;

namespace HelpDeskSystem.DTOs.Auth
{
    public sealed class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}
