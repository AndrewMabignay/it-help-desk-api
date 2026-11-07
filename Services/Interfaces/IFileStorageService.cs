namespace HelpDeskSystem.Services.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string filePath,
        CancellationToken cancellationToken);
}
