using System.Threading.Tasks;
using blogest.application.DTOs.responses;
using blogest.application.Features.queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;

[ApiController]
[Route("api/[controller]")]
public class CommentsQueryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CommentsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("Get/{CommentId}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid CommentId)
    {
        GetCommentByIdQuery query = new GetCommentByIdQuery(CommentId);
        GetCommentByIdResponse response = await _mediator.Send(query);

        return Ok(response);
    }
}