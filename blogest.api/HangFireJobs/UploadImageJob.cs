using blogest.application.Features.commands.UploadImage;
using blogest.application.Interfaces.services;

namespace blogest.api.HangFireJobs;

public class UploadImageJob
{
    private readonly IImageStorageService _imageStorageSevice;
    public UploadImageJob(IImageStorageService imageStorageService)
    {
        _imageStorageSevice = imageStorageService;
    }
    public async Task ExecuteAsync(string filePath, string fileName, string contentType, Guid userId)
    {
        using var stream = File.OpenRead(filePath);
        UploadImageCommand command = new UploadImageCommand(stream, fileName, contentType, userId);
        await _imageStorageSevice.UploadImageAsync(command);

        File.Delete(filePath);
    }
}