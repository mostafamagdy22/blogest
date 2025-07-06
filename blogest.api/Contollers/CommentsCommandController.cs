using blogest.application.DTOs.requests;
using blogest.application.DTOs.responses.Comments;
using blogest.application.Features.commands.Comments;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/[controller]")]
public class CommentsCommandController : ControllerBase
{
    private readonly IMediator _mediator;
    public CommentsCommandController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Creates a new comment on a post.
    /// </summary>
    /// <param name="createCommentCommand">Comment creation data.</param>
    /// <returns>200 if successful, 400 if failed.</returns>
    [HttpPost("Create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand createCommentCommand)
    {
        CreateCommentResponse response = await _mediator.Send(createCommentCommand);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Deletes a comment by its ID.
    /// </summary>
    /// <param name="commentId">The ID of the comment to delete.</param>
    /// <returns>200 if successful, 400 if failed.</returns>
    [HttpDelete("delete/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
    {
        DeleteCommentCommand command = new DeleteCommentCommand(commentId);
        DeleteCommentResponse response = await _mediator.Send(command);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Updates the content of a comment.
    /// </summary>
    /// <param name="commentId">The ID of the comment to update.</param>
    /// <param name="content">The new content for the comment.</param>
    /// <returns>200 if successful, 400 if failed.</returns>
    [HttpPut("update/{commentId}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentRequestDto content)
    {
        UpdateCommentCommand command = new UpdateCommentCommand(commentId, content.Content);
        UpdateCommentResponse response = await _mediator.Send(command);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }
}