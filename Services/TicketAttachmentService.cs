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

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        var isITStaff = await _userManager.IsInRoleAsync(user, "ITStaff");
        var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

        if (!isAdmin && !isITStaff && !isEmployee)
            throw new UnauthorizedAccessException(
                "You are not authorized to view attachments.");

        if (isEmployee && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException(
                "You can only view attachments on your own tickets.");

        var attachments = await _dbContext.TicketAttachments
            .AsNoTracking()
            .Include(a => a.UploadedBy)
            .Where(a => a.TicketId == ticketId)
            .OrderByDescending(a => a.UploadedAt)
            .ToListAsync(cancellationToken);

        return attachments
            .Select(a => a.ToResponseDto())
            .ToList();
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

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        var isITStaff = await _userManager.IsInRoleAsync(user, "ITStaff");
        var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

        if (!isAdmin && !isITStaff && !isEmployee)
            throw new UnauthorizedAccessException(
                "You are not authorized to upload attachments.");

        if (isEmployee && ticket.RequesterId != userId)
            throw new UnauthorizedAccessException(
                "You can only upload attachments to your own tickets.");

        var file = dto.File;

        if (file is null || file.Length == 0)
            throw new ArgumentException("Please select a valid file.");

        if (file.Length > MaxFileSize)
            throw new ArgumentException("Maximum file size is 5 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException("File type is not allowed.");

        var isValidFile = await HasValidFileSignatureAsync(
            file,
            extension,
            cancellationToken);

        if (!isValidFile)
        {
            throw new ArgumentException(
                "The uploaded file content does not match its extension.");
        }

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

    public async Task<TicketAttachmentDownloadDto> DownloadAsync(
        Guid ticketId,
        Guid attachmentId,
        string userId,
        CancellationToken cancellationToken)
    {
        // Check if ticket exists
        var ticket = await _dbContext.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Id == ticketId,
                cancellationToken);

        if (ticket is null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        // Check if user exists
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }

        // Check user roles
        var roles = await _userManager.GetRolesAsync(user);

        var isAdmin = roles.Contains("Admin");
        var isITStaff = roles.Contains("ITStaff");
        var isEmployee = roles.Contains("Employee");

        if (!isAdmin && !isITStaff && !isEmployee)
        {
            throw new UnauthorizedAccessException(
                "You are not allowed to download attachments.");
        }

        // Employees can only access their own ticket attachments
        if (isEmployee && ticket.RequesterId != userId)
        {
            throw new UnauthorizedAccessException(
                "You can only download attachments from your own tickets.");
        }

        // Find attachment belonging to the specified ticket
        var attachment = await _dbContext.TicketAttachments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.Id == attachmentId && a.TicketId == ticketId,
                cancellationToken);

        if (attachment is null)
        {
            throw new KeyNotFoundException("Attachment not found.");
        }

        // Open file stream
        var stream = await _fileStorage.OpenReadAsync(
            attachment.FilePath,
            cancellationToken);

        return new TicketAttachmentDownloadDto
        {
            Content = stream,
            FileName = Path.GetFileName(attachment.OriginalFileName),
            ContentType = "application/octet-stream"
        };
    }

    private static async Task<bool> HasValidFileSignatureAsync(
        IFormFile file,
        string extension,
        CancellationToken cancellationToken)
    {
        var signatures = new Dictionary<string, byte[][]>
        {
            [".jpg"] =
            [
                [0xFF, 0xD8, 0xFF]
            ],
            [".jpeg"] =
            [
                [0xFF, 0xD8, 0xFF]
            ],
            [".png"] =
            [
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]
            ],
            [".pdf"] =
            [
                [0x25, 0x50, 0x44, 0x46, 0x2D]
            ]
        };

        // Signature validation for formats with known signatures.
        if (!signatures.TryGetValue(extension, out var expectedSignatures))
        {
            // DOC, DOCX, and TXT require additional format-specific validation.
            return extension is ".doc" or ".docx" or ".txt";
        }

        var header = new byte[expectedSignatures.Max(s => s.Length)];

        await using var stream = file.OpenReadStream();

        var bytesRead = await stream.ReadAsync(
            header.AsMemory(0, header.Length),
            cancellationToken);

        return expectedSignatures.Any(signature =>
            bytesRead >= signature.Length &&
            header.AsSpan(0, signature.Length).SequenceEqual(signature));
    }
}
