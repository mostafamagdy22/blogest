using System.Net;
using blogest.application.DTOs.responses.Comments;
using blogest.application.Features.queries.Comments;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class CommentsQueryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CommentsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Retrieves a comment by its unique identifier.
    /// </summary>
    /// <param name="CommentId">The unique identifier of the comment.</param>
    /// <returns>The comment details if found, otherwise 404.</returns>
    [HttpGet("Get/{CommentId}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid CommentId)
    {
        GetCommentByIdQuery query = new GetCommentByIdQuery(CommentId);
        GetCommentByIdResponse response = await _mediator.Send(query);

        return Ok(response);
    }
    /// <summary>
    /// Retrieves all comments for a specific post with pagination.
    /// </summary>
    /// <param name="PostId">The unique identifier of the post.</param>
    /// <param name="pageNumber">The page number (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <returns>Paginated list of comments for the post.</returns>
    [HttpGet("GetAll/{PostId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCommentsByPostId([FromRoute] Guid PostId,int pageNumber = 1,int pageSize = 10)
    {
        GetCommentsByPostIdQuery query = new GetCommentsByPostIdQuery(PostId,pageNumber,pageSize);
        GetCommentsOnPostResponse response = await _mediator.Send(query);
        return Ok(response);
    }
    /// <summary>
    /// Retrieves all comments made by a specific user.
    /// </summary>
    /// <param name="UserId">The unique identifier of the user.</param>
    /// <returns>Paginated list of comments made by the user.</returns>
    [HttpGet("GetAllCommentsOfUser/{UserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCommentsOfUser([FromRoute] Guid UserId)
    {
        GetCommentsOfUserQuery query = new GetCommentsOfUserQuery(UserId);
        GetCommentsOfUserResponse response = await _mediator.Send(query);
        return Ok(response);
    }

}