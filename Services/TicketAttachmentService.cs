using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.TicketAttachments;
using HelpDeskSystem.Mappings;
using HelpDeskSystem.Models;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class TicketAttachmentService : ITicketAttachmentService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileStorageService _fileStorage;

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
    [
        ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".txt"
    ];

    public TicketAttachmentService(
        ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IFileStorageService fileStorage)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _fileStorage = fileStorage;
    }

    public async Task<IEnumerable<TicketAttachmentResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

        if (isEmployee && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException("You cannot access this ticket.");

        return await _dbContext.TicketAttachments
            .AsNoTracking()
            .Include(a => a.UploadedBy)
            .Where(a => a.TicketId == ticketId)
            .OrderByDescending(a => a.UploadedAt)
            .Select(a => a.ToResponseDto())
            .ToListAsync(cancellationToken);
    }

    public async Task<TicketAttachmentResponseDto> UploadAsync(
        Guid ticketId,
        CreateTicketAttachmentDto dto,
        string userId,
        CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            throw new KeyNotFoundException("Ticket not found.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new UnauthorizedAccessException("User not found.");

        var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

        if (isEmployee && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException("You cannot upload to this ticket.");

        var file = dto.File;

        if (file is null || file.Length == 0)
            throw new ArgumentException("Please select a valid file.");

        if (file.Length > MaxFileSize)
            throw new ArgumentException("Maximum file size is 5 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException("File type is not allowed.");

        var filePath = await _fileStorage.UploadAsync(
            file,
            "TicketAttachments",
            cancellationToken);

        try
        {
            var attachment = new TicketAttachment
            {
                Id = Guid.NewGuid(),
                TicketId = ticketId,
                UploadedById = userId,
                OriginalFileName = Path.GetFileName(file.FileName),
                StoredFileName = Path.GetFileName(filePath),
                FilePath = filePath,
                ContentType = file.ContentType,
                FileSizeBytes = file.Length,
                UploadedAt = DateTime.UtcNow
            };

            _dbContext.TicketAttachments.Add(attachment);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _dbContext.Entry(attachment)
                .Reference(a => a.UploadedBy)
                .LoadAsync(cancellationToken);

            return attachment.ToResponseDto();
        }
        catch
        {
            await _fileStorage.DeleteAsync(filePath, CancellationToken.None);
            throw;
        }
    }
}
