using Microsoft.AspNetCore.Mvc;
using MediatR;
using blogest.application.Features.commands.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using blogest.application.DTOs.responses.Posts;
namespace blogest.api.Controllers
{
    [ApiVersion("1.0")]
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
            if (result is { isCreatedSuccessfully: false })
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("Delete/{postId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
            var command = new DeletePostCommand(postId);
            var result = await _mediator.Send(command);
            if (result is { IsSuccess: false })
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("DeleteByUser/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePostsByUser([FromRoute] Guid userId)
        {
            DeletePostsByUserCommand command = new DeletePostsByUserCommand(userId);
            DeletePostResponse response = await _mediator.Send(command);
            if (response is { IsSuccess: false })
                return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("Update/{postId}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostCommand command)
        {
            if (command.postId == Guid.Empty || command.postId != postId)
                command = new UpdatePostCommand(command.Title, command.Content, postId);

            UpdatePostResponse result = await _mediator.Send(command);
            if (result is { success: false })
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("{id}/categories")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePostCategories([FromRoute] Guid id, [FromBody] List<int> categoriesIds)
        {
            UpdatePostCategoriesCommand command = new UpdatePostCategoriesCommand(id, categoriesIds);
            UpdatePostCategoriesResponse response = await _mediator.Send(command);
            if (response is { IsSuccess: false })
                return BadRequest(response);
            return Ok(response);
        }
    }
}