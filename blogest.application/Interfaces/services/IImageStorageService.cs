using blogest.application.Features.commands.UploadImage;

namespace blogest.application.Interfaces.services;

public interface IImageStorageService
{
    public Task<string> UploadImageAsync(UploadImageCommand file);
}