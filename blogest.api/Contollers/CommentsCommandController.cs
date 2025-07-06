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
    [HttpPost("Create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand createCommentCommand)
    {
        CreateCommentResponse response = await _mediator.Send(createCommentCommand);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }

    [HttpDelete("delete/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
    {
        DeleteCommentCommand command = new DeleteCommentCommand(commentId);
        DeleteCommentResponse response = await _mediator.Send(command);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }

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