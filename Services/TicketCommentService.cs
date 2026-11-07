using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.TicketComments;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class TicketCommentService : ITicketCommentService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketCommentService(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<IEnumerable<TicketCommentResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Id == ticketId,
                cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        var isITStaff = await _userManager.IsInRoleAsync(user, "ITStaff");

        if (!isAdmin && !isITStaff && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException("You cannot access this ticket.");

        var query = _dbContext.TicketComments
            .AsNoTracking()
            .Where(c => c.TicketId == ticketId)
            .Include(c => c.User)
            .AsQueryable();

        if (!isAdmin && !isITStaff)
            query = query.Where(c => !c.IsInternal);

        var comments = await query
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);

        return comments.Select(c => c.ToResponseDto());
    }

    public async Task<TicketCommentResponseDto> CreateAsync(
        CreateTicketCommentDto request,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets
            .FirstOrDefaultAsync(
                t => t.Id == request.TicketId,
                cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        var isITStaff = await _userManager.IsInRoleAsync(user, "ITStaff");

        if (!isAdmin && !isITStaff && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException("You cannot comment on this ticket.");

        if (request.IsInternal && !isAdmin && !isITStaff)
            throw new UnauthorizedAccessException(
                "Only IT Staff or Admin can create internal notes.");

        var comment = new TicketComment
        {
            TicketId = request.TicketId,
            UserId = userId,
            Comment = request.Comment,
            IsInternal = request.IsInternal,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.TicketComments.Add(comment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        comment.User = user;

        return comment.ToResponseDto();
    }
}
