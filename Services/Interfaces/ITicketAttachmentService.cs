using HelpDeskSystem.DTOs.TicketAttachments;

namespace HelpDeskSystem.Services.Interfaces;

public interface ITicketAttachmentService
{
    Task<IEnumerable<TicketAttachmentResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken);

    Task<TicketAttachmentResponseDto> UploadAsync(
        Guid ticketId,
        CreateTicketAttachmentDto dto,
        string userId,
        CancellationToken cancellationToken);
}
