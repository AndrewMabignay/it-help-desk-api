using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.TicketWatchers;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class TicketWatcherService : ITicketWatcherService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketWatcherService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IEnumerable<TicketWatcherResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        await EnsureCanAccessTicketAsync(ticket, userId);

        var watchers = await _context.TicketWatchers
            .AsNoTracking()
            .Include(w => w.User)
            .Where(w => w.TicketId == ticketId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(cancellationToken);

        return watchers.Select(w => w.ToResponseDto());
    }

    public async Task<TicketWatcherResponseDto> AddAsync(
        Guid ticketId,
        CreateTicketWatcherDto request,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        await EnsureCanAccessTicketAsync(ticket, userId);

        var watcherUser = await _userManager.FindByIdAsync(request.UserId);

        if (watcherUser is null)
            throw new KeyNotFoundException("User not found.");

        var existingWatcher = await _context.TicketWatchers
            .AnyAsync(
                w => w.TicketId == ticketId && w.UserId == request.UserId, cancellationToken);

        if (existingWatcher)
            throw new InvalidOperationException("User is already watching this ticket.");

        var watcher = new TicketWatcher
        { 
            Id = Guid.NewGuid(),
            TicketId = ticketId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.TicketWatchers.Add(watcher);

        await _context.SaveChangesAsync(cancellationToken);

        await _context.Entry(watcher)
            .Reference(w => w.User)
            .LoadAsync(cancellationToken);

        return watcher.ToResponseDto();
    }

    public async Task RemoveAsync(
        Guid ticketId,
        string watcherUserId,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        await EnsureCanAccessTicketAsync(ticket, userId);

        var watcher = await _context.TicketWatchers
            .FirstOrDefaultAsync(
                w => w.TicketId == ticketId && w.UserId == watcherUserId,
                cancellationToken);

        if (watcher is null)
            throw new KeyNotFoundException("Ticket watcher not found.");

        _context.TicketWatchers.Remove(watcher);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureCanAccessTicketAsync(
        Ticket ticket,
        string userId) 
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

        if (isEmployee && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException("You cannot access this ticket.");
    }
}
