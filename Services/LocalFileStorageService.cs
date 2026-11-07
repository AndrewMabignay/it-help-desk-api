using HelpDeskSystem.Services.Interfaces;

namespace HelpDeskSystem.Services;

public sealed class LocalFileStorageService : IFileStorageService
{
    private readonly string _storagePath;

    public LocalFileStorageService(IWebHostEnvironment environment)
    {
        _storagePath = Path.Combine(
            environment.ContentRootPath,
            "App_Data"
        );
    }

    public async Task<string> UploadAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        var storedFileName = $"{Guid.NewGuid()}{extension}";

        var folderPath = Path.Combine(_storagePath, folder);

        Directory.CreateDirectory(folderPath);

        var fullPath = Path.Combine(folderPath, storedFileName);

        await using var stream = new FileStream(
            fullPath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None
        );

        await file.CopyToAsync(stream, cancellationToken);

        return Path.Combine(folder, storedFileName)
            .Replace("\\", "/");
    }

    public Task DeleteAsync(
        string filePath,
        CancellationToken cancellationToken)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_storagePath, filePath));

        var rootPath = Path.GetFullPath(_storagePath)
            + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(
            rootPath,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException(
                "Invalid file path."
            );
        }

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
