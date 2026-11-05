namespace HelpDeskSystem.DTOs.Users
{
    public sealed class UserResponseDto
    {
        public string Id { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public string FirstName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        public int? DepartmentId { get; init; }

        public IList<string> Roles { get; init; } = new List<string>();

        public bool IsActive { get; init; }
    }
}
