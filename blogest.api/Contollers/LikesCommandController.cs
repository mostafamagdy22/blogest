using blogest.application.DTOs.responses.Likes;
using blogest.application.Features.commands.Likes;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class LikesCommandController : ControllerBase
{
    private readonly IMediator _mediator;
    public LikesCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Adds a like to a post by the current user.
    /// </summary>
    /// <param name="postId">The ID of the post to like.</param>
    /// <returns>200 if successful, 400 if already liked or error.</returns>
    [HttpPost("AddLike/{postId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> AddLike([FromRoute] Guid postId)
    {
        var command = new AddLikeCommand(postId);
        AddLikeResponse response = await _mediator.Send(command);
        return response.success ? Ok(response) : BadRequest(response.message);
    }

    /// <summary>
    /// Removes a like from a post by the current user.
    /// </summary>
    /// <param name="postId">The ID of the post to unlike.</param>
    /// <returns>200 if successful, 404 if not found.</returns>
    [HttpDelete("UnLike/{postId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UnLike([FromRoute] Guid postId)
    {
        var command = new UnLikeCommand(postId);
        UnLikeResponse response = await _mediator.Send(command);
        return response.success ? Ok(response) : NotFound(response.message);
    }
}