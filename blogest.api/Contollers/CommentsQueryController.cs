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
[Route("api/[controller]")]
public class CommentsQueryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CommentsQueryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Gets a comment by its ID.
    /// </summary>
    /// <param name="CommentId">The ID of the comment.</param>
    /// <returns>The comment details.</returns>
    [HttpGet("Get/{CommentId}")]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid CommentId)
    {
        GetCommentByIdQuery query = new GetCommentByIdQuery(CommentId);
        GetCommentByIdResponse response = await _mediator.Send(query);

        return Ok(response);
    }
    /// <summary>
    /// Gets all comments on a post (paginated).
    /// </summary>
    /// <param name="PostId">The ID of the post.</param>
    /// <param name="pageNumber">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 10).</param>
    /// <returns>Paginated list of comments.</returns>
    [HttpGet("GetAll/{PostId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCommentsByPostId([FromRoute] Guid PostId,int pageNumber = 1,int pageSize = 10)
    {
        GetCommentsByPostIdQuery query = new GetCommentsByPostIdQuery(PostId,pageNumber,pageSize);
        GetCommentsOnPostResponse response = await _mediator.Send(query);
        return Ok(response);
    }
    /// <summary>
    /// Gets all comments made by a user.
    /// </summary>
    /// <param name="UserId">The ID of the user.</param>
    /// <returns>List of comments by user.</returns>
    [HttpGet("GetAllCommentsOfUser/{UserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCommentsOfUser([FromRoute] Guid UserId)
    {
        GetCommentsOfUserQuery query = new GetCommentsOfUserQuery(UserId);
        GetCommentsOfUserResponse response = await _mediator.Send(query);
        return Ok(response);
    }

}