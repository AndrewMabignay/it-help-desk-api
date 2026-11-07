using HelpDeskSystem.Models.Enums;

namespace HelpDeskSystem.DTOs.Tickets;

public sealed class TicketQueryDto
{
    // Search
    public string? Search { get; init; }

    // Filters
    public TicketStatus? Status { get; init; }

    public Guid? CategoryId { get; init; }

    public Guid? PriorityId { get; init; }

    public string? RequesterId { get; init; }

    public string? AssignedToId { get; init; }

    // Sorting
    public string SortBy { get; init; } = "CreatedAt";

    public bool isDescending { get; init; } = true;

    // Pagination
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
