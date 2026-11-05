namespace HelpDeskSystem.DTOs.Auth
{
    public sealed class CurrentUserDto
    {
        public string Id { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public IList<string> Roles { get; init; } = new List<string>();
    }
}
