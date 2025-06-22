using blogest.application.DTOs.requests;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;

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
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommand createCommentCommand)
    {
        CreateCommentResponse response = await _mediator.Send(createCommentCommand);
        return Ok(response);
    }
    [HttpDelete("delete/{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
    {
        DeleteCommentCommand command = new DeleteCommentCommand(commentId);
        DeleteCommentResponse response = await _mediator.Send(command);
        return Ok(response);
    }
    [HttpPut("update/{commentId}")]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentRequestDto content)
    {
        UpdateCommentCommand command = new UpdateCommentCommand(commentId,content.Content);
        UpdateCommentResponse response = await _mediator.Send(command);
        return Ok(response);
    }
}