using blogest.application.DTOs.responses.Likes;
using blogest.application.Features.queries.Likes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/[controller]")]
    public class LikesQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LikesQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Gets all likes for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>List of users who liked the post.</returns>
        [HttpGet("GetPostLikes/{postId}")]
        public async Task<IActionResult> GetPostLikes([FromRoute] Guid postId)
        {
            GetPostLikesQuery query = new GetPostLikesQuery(postId);
            GetPostLikesResponse response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}