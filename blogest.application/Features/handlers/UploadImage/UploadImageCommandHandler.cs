using blogest.application.DTOs.responses.UploadImage;
using blogest.application.Features.commands.UploadImage;
using blogest.application.Interfaces.services;
using MediatR;

namespace blogest.application.Features.handlers.UploadImage;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, UploadImageResponse>
{
    private readonly IImageStorageService _service;
    public UploadImageCommandHandler(IImageStorageService service)
    {
        _service = service;
    }
    public async Task<UploadImageResponse> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var file = request.FileStream;
        if (file == null || file.Length == 0)
            return new UploadImageResponse(null,false,"file is empty");

        string imageUrl = await _service.UploadImageAsync(request);

        return new UploadImageResponse(imageUrl,true,null);
    }
}