using blogest.application.DTOs.responses.Posts;
using blogest.application.Features.queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PostsQueryController : ControllerBase
{
    private readonly IMediator _mediator;
    public PostsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>The post details if found, otherwise 404.</returns>
    [HttpGet("get/{postId}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] Guid postId)
    {
        GetPostByIdQuery query = new GetPostByIdQuery(postId);
        GetPostResponse result = await _mediator.Send(query);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all posts in a specific category with pagination and optional includes.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category.</param>
    /// <param name="pageNumber">The page number (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="include">Related data to include (optional, e.g., comments).</param>
    /// <returns>Paginated list of posts in the category.</returns>
    [HttpGet("getAllByCategory/{categoryId}")]
    public async Task<IActionResult> GetAllPostsByCategory([FromRoute] int categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? include = "")
    {
        GetPostsByCategoryQuery query = new GetPostsByCategoryQuery(categoryId, pageNumber, pageSize, include);
        GetPostsByCategoryResponse response = await _mediator.Send(query);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }

    /// <summary>
    /// Retrieves all posts created by a specific user with pagination and optional includes.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="include">Related data to include (optional, e.g., comments).</param>
    /// <param name="pageNumber">The page number (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>Paginated list of posts created by the user.</returns>
    [HttpGet("getAllByUser/{userId}")]
    public async Task<IActionResult> GetAllPostsByUser([FromRoute] Guid userId, [FromQuery] string? include, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        GetPostsByUserIdQuery query = new GetPostsByUserIdQuery(userId, include, pageNumber, pageSize);
        GetPostsByCategoryResponse response = await _mediator.Send(query);
        if (response is { IsSuccess: false })
            return BadRequest(response);
        return Ok(response);
    }
    /// <summary>
    /// Gets all posts liked by a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="include">Related data to include (optional).</param>
    /// <param name="pageNumber">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 10).</param>
    /// <returns>Paginated list of liked posts.</returns>
    [HttpGet("liked-by-user/{userId}")]
    public async Task<IActionResult> GetPostsLikedByUser([FromRoute] Guid userId,[FromQuery] string? include,[FromQuery]int pageNumber = 1,[FromQuery]int pageSize = 10)
    {
        GetUserLikesQuery query = new GetUserLikesQuery(userId,include,pageNumber,pageSize);
        GetUserLikesResponse response = await _mediator.Send(query);
        return Ok(response);
    }
}
