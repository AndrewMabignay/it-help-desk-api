using HelpDeskSystem.DTOs;
using HelpDeskSystem.DTOs.Tickets;

namespace HelpDeskSystem.Services.Interfaces;

public interface ITicketService
{
    // Get all tickets
    Task<PagedResponseDto<TicketResponseDto>> GetAllAsync(
        TicketQueryDto query,
        CancellationToken cancellationToken);

    // Get ticket by ID
    Task<TicketResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    // Create ticket
    Task<TicketResponseDto> CreateAsync(
        CreateTicketRequestDto request,
        string requesterId,
        CancellationToken cancellationToken);

    // Update ticket
    Task<TicketResponseDto?> UpdateAsync(
        Guid id,
        UpdateTicketRequestDto request,
        CancellationToken cancellationToken);

    // Assign ticket
    Task<bool> AssignAsync(
        Guid id,
        AssignTicketRequestDto request,
        CancellationToken cancellationToken);

    // Update ticket status
    Task<bool> UpdateStatusAsync(
        Guid id,
        UpdateTicketStatusRequestDto request,
        CancellationToken cancellationToken);
}
