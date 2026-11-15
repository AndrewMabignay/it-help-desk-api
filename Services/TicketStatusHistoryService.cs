using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.TicketStatusHistories;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class TicketStatusHistoryService : ITicketStatusHistoryService
{
    private readonly ApplicationDbContext _context;

    public TicketStatusHistoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketStatusHistoryResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        // Employee can only access their own tickets.
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

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
            throw new UnauthorizedAccessException("You cannot access this ticket.");

        var history = await _context.TicketStatusHistories
            .Include(h => h.ChangedBy)
            .Where(h => h.TicketId == ticketId)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync(cancellationToken);

        return history.Select(h => h.ToResponseDto());
    }
}
