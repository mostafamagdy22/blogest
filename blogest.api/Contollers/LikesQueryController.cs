using blogest.application.DTOs.responses.Likes;
using blogest.application.Features.queries.Likes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace blogest.api.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesQueryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LikesQueryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetPostLikes/{postId}")]
        public async Task<IActionResult> GetPostLikes([FromRoute] Guid postId)
        {
            GetPostLikesQuery query = new GetPostLikesQuery(postId);
            GetPostLikesResponse response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}