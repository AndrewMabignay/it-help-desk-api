using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.TicketResolutions;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Models.Enums;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class TicketResolutionService : ITicketResolutionService
{
    private readonly ApplicationDbContext _context;

    public TicketResolutionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TicketResolutionResponseDto> CreateAsync(
        Guid ticketId,
        CreateTicketResolutionDto request,
        string resolvedById,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(
                t => t.Id == ticketId,
                cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        if (ticket.Status != TicketStatus.Resolved)
            throw new InvalidOperationException(
                "Resolution details can only be added to resolved tickets.");

        var existingResolution = await _context.TicketResolutions
           .AnyAsync(
               r => r.TicketId == ticketId,
               cancellationToken);

        if (existingResolution)
            throw new InvalidOperationException(
                "This ticket already has a resolution record.");

        var userExists = await _context.Users
            .AnyAsync(
                u => u.Id == resolvedById,
                cancellationToken);

        if (!userExists)
            throw new UnauthorizedAccessException("User not found.");

        var resolution = new TicketResolution
        { 
            Id = Guid.NewGuid(),
            TicketId = ticketId,
            ResolvedById = resolvedById,
            ResolutionSummary = request.ResolutionSummary,
            ResolutionDetails = request.ResolutionDetails,
            ResolvedAt = DateTime.UtcNow
        };

        _context.TicketResolutions.Add(resolution);

        await _context.SaveChangesAsync(cancellationToken);

        var result = await _context.TicketResolutions
            .Include(r => r.ResolvedBy)
            .FirstAsync(
                r => r.Id == resolution.Id,
                cancellationToken);

        return result.ToResponseDto();
    }

    public async Task<TicketResolutionResponseDto?> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(
                t => t.Id == ticketId,
                cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        var isEmployee = await _context.UserRoles
            .Join(
                _context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => new { ur.UserId, r.Name })
            .AnyAsync(
                x => x.UserId == userId && x.Name == "Employee",
                cancellationToken);

        if (isEmployee && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException(
                "You cannot access this ticket.");

        var resolution = await _context.TicketResolutions
            .Include(r => r.ResolvedBy)
            .FirstOrDefaultAsync(
                r => r.TicketId == ticketId,
                cancellationToken);

        return resolution?.ToResponseDto();
    }
}
