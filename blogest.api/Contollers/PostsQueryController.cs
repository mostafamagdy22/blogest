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
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] Guid postId)
    {
        GetPostByIdQuery query = new GetPostByIdQuery(postId);
        GetPostResponse result = await _mediator.Send(query);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("getAllByCategory/{categoryId}")]
    public async Task<IActionResult> GetAllPostsByCategory([FromRoute] int categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? include = "")
    {
        GetPostsByCategoryQuery query = new GetPostsByCategoryQuery(categoryId, pageNumber, pageSize, include);
        GetPostsByCategoryResponse response = await _mediator.Send(query);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("getAllByUser/{userId}")]
    public async Task<IActionResult> GetAllPostsByUser([FromRoute] Guid userId, [FromQuery] string? include, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        GetPostsByUserIdQuery query = new GetPostsByUserIdQuery(userId, include, pageNumber, pageSize);
        GetPostsByCategoryResponse response = await _mediator.Send(query);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }
    [HttpGet("liked-by-user/{userId}")]
    public async Task<IActionResult> GetPostsLikedByUser([FromRoute] Guid userId,[FromQuery] string? include,[FromQuery]int pageNumber = 1,[FromQuery]int pageSize = 10)
    {
        GetUserLikesQuery query = new GetUserLikesQuery(userId,include,pageNumber,pageSize);
        GetUserLikesResponse response = await _mediator.Send(query);
        return Ok(response);
    }
}
