using blogest.application.Features.commands.UploadImage;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Configuration;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace blogest.infrastructure.Services;

public class CloudinaryStorageService : IImageStorageService
{
    private readonly CloudinarySettings _cloudinary;
    private readonly BlogCommandContext _blogCommandContext;
    public CloudinaryStorageService(BlogCommandContext blogCommandContext, IOptions<CloudinarySettings> config)
    {
        _blogCommandContext = blogCommandContext;
        _cloudinary = config.Value;
    }
    public async Task<string> UploadImageAsync(UploadImageCommand file)
    {
        var account = new Account(_cloudinary.CloudName, _cloudinary.ApiKey, _cloudinary.ApiSecret);

        var cloudinary = new Cloudinary(account);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.FileStream),
            Folder = "blogest-uploads"
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            AppUser user = await _blogCommandContext.Users.FindAsync(file.UserId);
            user.Image = uploadResult.SecureUrl.ToString();
            await _blogCommandContext.SaveChangesAsync();
            return uploadResult.SecureUrl.ToString();
        }

        throw new ApplicationException("Failed to upload the photo");
    }
}