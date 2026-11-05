namespace HelpDeskSystem.DTOs.Users
{
    public sealed class PagedResponseDto<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public int TotalCount { get; init; }

        public int TotalPages => 
            PageSize > 0
                ? (int)Math.Ceiling((double)TotalCount / PageSize)
                : 0;
    }
}