using blogest.application.DTOs.responses.Posts;
using blogest.application.Features.queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;

[ApiController]
[Route("api/[controller]")]
public class PostsQueryController : ControllerBase
{
    private readonly IMediator _mediator;
    public PostsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("get/{postId}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute]Guid postId)
    {
        GetPostByIdQuery query = new GetPostByIdQuery(postId);
        GetPostResponse result = await _mediator.Send(query);
        return Ok(result);
    }
}
