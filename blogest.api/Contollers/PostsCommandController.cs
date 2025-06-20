using Microsoft.AspNetCore.Mvc;
using MediatR;
using blogest.application.Features.commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using blogest.application.DTOs.responses;
namespace blogest.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostsCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("Delete/{postId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
            var command = new DeletePostCommand(postId);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("Update/{postId}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId,[FromBody] UpdatePostCommand command)
        {
            if (command.postId == Guid.Empty || command.postId != postId)
                command = new UpdatePostCommand(command.Title,command.Content,postId);

            UpdatePostResponse result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}