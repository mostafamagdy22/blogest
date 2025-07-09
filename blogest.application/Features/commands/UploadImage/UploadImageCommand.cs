using blogest.application.DTOs.responses.UploadImage;
using MediatR;
namespace blogest.application.Features.commands.UploadImage;

public record UploadImageCommand(Stream FileStream,string FileName,string ContentType,Guid UserId) : IRequest<UploadImageResponse>;