using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs;
using HelpDeskSystem.DTOs.Priorities;
using HelpDeskSystem.DTOs.Tickets;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Models.Enums;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class TicketService : ITicketService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public TicketService(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    // Get all tasks (server-side paginated and search)
    public async Task<PagedResponseDto<TicketResponseDto>> GetAllAsync(
        TicketQueryDto query,
        CancellationToken cancellationToken)
    {
        var tickets = _dbContext.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Requester)
            .Include(t => t.AssignedTo)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            tickets = tickets.Where(t =>
                t.TicketNumber.Contains(search) ||
                t.Subject.Contains(search) ||
                t.Description.Contains(search));
        }

        // Filters
        if (query.Status.HasValue)
            tickets = tickets.Where(t => t.Status == query.Status.Value);
        
        if (query.CategoryId.HasValue)
            tickets = tickets.Where(t => t.CategoryId == query.CategoryId.Value);
        
        if (query.PriorityId.HasValue)
            tickets = tickets.Where(t => t.PriorityId == query.PriorityId.Value);

        if (!string.IsNullOrWhiteSpace(query.RequesterId))
            tickets = tickets.Where(t => t.RequesterId == query.RequesterId);

        if (!string.IsNullOrWhiteSpace(query.AssignedToId))
            tickets = tickets.Where(t => t.AssignedToId == query.AssignedToId);

        // Total Count
        var totalCount = await tickets.CountAsync(cancellationToken);

        // Pagination
        var pageNumber = Math.Max(query.PageNumber, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        // Sorting
        tickets = query.SortBy.ToLower() switch
        {
            "ticketnumber" => query.isDescending
                ? tickets.OrderByDescending(t => t.TicketNumber)
                : tickets.OrderBy(t => t.TicketNumber),

            "subject" => query.isDescending
                ? tickets.OrderByDescending(t => t.Subject)
                : tickets.OrderBy(t => t.Subject),

            "status" => query.isDescending
                ? tickets.OrderByDescending(t => t.Status)
                : tickets.OrderBy(t => t.Status),

            _ => query.isDescending
                ? tickets.OrderByDescending(t => t.CreatedAt)
                : tickets.OrderBy(t => t.CreatedAt)
        };

        // Fetch Records
        var items = await tickets
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponseDto<TicketResponseDto>
        { 
            Items = items.Select(t => t.ToResponseDto()).ToList(),
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    // Get ticket by ID
    public async Task<TicketResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Requester)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) 
            return null;

        return ticket.ToResponseDto();
    }

    // Create ticket
    public async Task<TicketResponseDto> CreateAsync(
        CreateTicketRequestDto request,
        string requesterId,
        CancellationToken cancellationToken)
    {
        // Validate Category
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId);

        if (category == null)
            throw new KeyNotFoundException("Category not found.");

        // Validate Priority
        var priority = await _dbContext.Priorities
            .FirstOrDefaultAsync(p => p.Id == request.PriorityId);

        if (priority == null)
            throw new KeyNotFoundException("Priority not found.");

        // Validate Requester
        var requester = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == requesterId);

        if (requester == null)
            throw new KeyNotFoundException("Requester not found.");

        // Generate Ticket ID and Number
        var ticketId = Guid.NewGuid();

        var ticketNumber = $"TKT-{DateTime.UtcNow:yyyyMMdd}-{ticketId.ToString("N")[..8].ToUpperInvariant()}";

        // Create Ticket
        var ticket = new Ticket
        {
            Id = ticketId,
            TicketNumber = ticketNumber,
            Subject = request.Subject,
            Description = request.Description,

            CategoryId = request.CategoryId,
            PriorityId = request.PriorityId,

            RequesterId = requesterId,

            Status = TicketStatus.Open,

            CreatedAt = DateTime.UtcNow,

            Category = category,
            Priority = priority,
            Requester = requester
        };

        // Save to Database
        _dbContext.Tickets.Add(ticket);

        await _dbContext.SaveChangesAsync();

        return ticket.ToResponseDto();
    }

    // Update ticket
    public async Task<TicketResponseDto?> UpdateAsync(
        Guid id,
        UpdateTicketRequestDto request,
        CancellationToken cancellationToken)
    {
        // Find Ticket
        var ticket = await _dbContext.Tickets
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Requester)
            .Include(t => t.AssignedTo)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            return null;

        // Validate Category
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId);

        if (category == null)
            throw new KeyNotFoundException("Category not found.");

        // Validate Priority
        var priority = await _dbContext.Priorities
            .FirstOrDefaultAsync(p => p.Id == request.PriorityId);

        if (priority == null)
            throw new KeyNotFoundException("Priority not found.");

        // Update Ticket Information
        ticket.Subject = request.Subject;
        ticket.Description = request.Description;

        ticket.CategoryId = request.CategoryId;
        ticket.Category = category;

        ticket.PriorityId = request.PriorityId;
        ticket.Priority = priority;

        ticket.UpdatedAt = DateTime.UtcNow;

        // Save Changes
        await _dbContext.SaveChangesAsync();

        return ticket.ToResponseDto();
    }

    public async Task<bool> AssignAsync(
        Guid id,
        AssignTicketRequestDto request,
        CancellationToken cancellationToken)
    {
        // Find ticket
        var ticket = await _dbContext.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            return false;

        // Find Assigned User
        var assignedUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.AssignedToId);

        if (assignedUser == null)
            throw new KeyNotFoundException("Assigned user not found.");

        // Validate User Status
        if (!assignedUser.IsActive)
            throw new InvalidOperationException("Cannot assign ticket to an inactive user.");

        // Validate User Role
        var isAuthorized = await _userManager.IsInRoleAsync(
            assignedUser, "Admin");

        if (!isAuthorized)
        {
            isAuthorized = await _userManager.IsInRoleAsync(
                assignedUser, "ITStaff");
        }

        if (!isAuthorized)
            throw new InvalidCastException("User is not authorized to receive tickets.");

        // Assign Ticket
        ticket.AssignedToId = assignedUser.Id;
        ticket.UpdatedAt = DateTime.UtcNow;

        // Save Changes
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateStatusAsync(
        Guid id,
        UpdateTicketStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        // Find Ticket
        var ticket = await _dbContext.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            return false;

        // Validate Status Transition
        var isValidTransition = (ticket.Status, request.Status) switch
        {
            (TicketStatus.Open, TicketStatus.InProgress) => true,
            (TicketStatus.InProgress, TicketStatus.Resolved) => true,
            (TicketStatus.Resolved, TicketStatus.Closed) => true,
            _ => false
        };

        if (!isValidTransition)
            throw new InvalidOperationException($"Cannot change ticket status from {ticket.Status} to {request.Status}.");

        // Update Status
        ticket.Status = request.Status;
        ticket.UpdatedAt = DateTime.UtcNow;

        // Update Lifecycle Timestamps
        switch (request.Status)
        {
            case TicketStatus.Resolved:
                ticket.ResolvedAt = DateTime.UtcNow;
                break;

            case TicketStatus.Closed:
                ticket.ClosedAt = DateTime.UtcNow;
                break;
        }

        // Save Changes
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
