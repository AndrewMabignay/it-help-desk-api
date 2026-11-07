using HelpDeskSystem.DTOs.TicketComments;

namespace HelpDeskSystem.Services.Interfaces;

public interface ITicketCommentService
{
    Task<IEnumerable<TicketCommentResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken);

    Task<TicketCommentResponseDto> CreateAsync(
        CreateTicketCommentDto request,
        string userId,
        CancellationToken cancellationToken);
}
