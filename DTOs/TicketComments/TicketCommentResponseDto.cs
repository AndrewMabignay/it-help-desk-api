namespace HelpDeskSystem.DTOs.TicketComments;

public sealed class TicketCommentResponseDto
{
    public int Id { get; init; }

    public Guid TicketId { get; init; }

    public string UserId { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public string Comment { get; init; } = string.Empty;

    public bool IsInternal { get; init; }

    public DateTime CreatedAt { get; init; }
}
