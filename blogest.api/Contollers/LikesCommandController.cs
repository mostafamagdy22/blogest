using blogest.application.DTOs.responses.Likes;
using blogest.application.Features.commands.Likes;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/[controller]")]
public class LikesCommandController : ControllerBase
{
    private readonly IMediator _mediator;
    public LikesCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("AddLike/{postId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddLike([FromRoute] Guid postId)
    {
        var command = new AddLikeCommand(postId);
        AddLikeResponse response = await _mediator.Send(command);
        return response.success ? Ok(response) : BadRequest(response.message);
    }

    [HttpDelete("UnLike/{postId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UnLike([FromRoute] Guid postId)
    {
        var command = new UnLikeCommand(postId);
        UnLikeResponse response = await _mediator.Send(command);
        return response.success ? Ok(response) : NotFound(response.message);
    }
}