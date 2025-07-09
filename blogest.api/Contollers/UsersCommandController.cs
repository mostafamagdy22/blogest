using System.Security.Claims;
using blogest.api.DTO;
using blogest.api.HangFireJobs;
using blogest.application.DTOs.responses.UploadImage;
using blogest.application.Features.commands.UploadImage;
using blogest.application.Interfaces.services;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace blogest.api.Contollers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersCommandController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("upload-photo")]
    [Consumes("multipart/form-data")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageDto dto, [FromServices] IBackgroundJobClient backgroundJobClient)
    {
        var tempFilePath = Path.GetTempFileName();
        using (var stream = System.IO.File.Create(tempFilePath))
        {
            dto.File.CopyTo(stream);
        }
        Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        backgroundJobClient.Enqueue<UploadImageJob>(job => job.ExecuteAsync(tempFilePath,dto.File.FileName,dto.File.ContentType,userId));

        return Ok(new {Message = "Photo uploaded successfully"});
    }
}