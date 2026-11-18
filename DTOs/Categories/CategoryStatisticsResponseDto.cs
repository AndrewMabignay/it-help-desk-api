namespace HelpDeskSystem.DTOs.Categories;

public sealed class CategoryStatisticsResponseDto
{
    public int TotalCount { get; init; }

    public int ActiveCount { get; init; }

    public int InactiveCount { get; init; }
}
