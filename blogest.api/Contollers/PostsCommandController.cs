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
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PostsCommandController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostsCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="command">Post creation data.</param>
        /// <returns>200 if successful, 400 if failed.</returns>
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var result = await _mediator.Send(command);
            if (result is { isCreatedSuccessfully: false })
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        /// <returns>200 if successful, 400 if failed.</returns>
        [HttpDelete("Delete/{postId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy = "CanEditPost")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
            var command = new DeletePostCommand(postId);
            var result = await _mediator.Send(command);
            if (result is { IsSuccess: false })
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Deletes all posts by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>200 if successful, 400 if failed.</returns>
        [HttpDelete("DeleteByUser/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy = "CanEditPost")]
        public async Task<IActionResult> DeletePostsByUser([FromRoute] Guid userId)
        {
            DeletePostsByUserCommand command = new DeletePostsByUserCommand(userId);
            DeletePostResponse response = await _mediator.Send(command);
            if (response is { IsSuccess: false })
                return BadRequest(response);
            return Ok(response);
        }
        /// <summary>
        /// Updates a post's title and content.
        /// </summary>
        /// <param name="postId">The ID of the post to update.</param>
        /// <param name="command">The new post data.</param>
        /// <returns>200 if successful, 400 if failed.</returns>
        [HttpPost("Update/{postId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy = "CanEditPost")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostCommand command)
        {
            if (command.postId == Guid.Empty || command.postId != postId)
                command = new UpdatePostCommand(command.Title, command.Content, postId);

            UpdatePostResponse result = await _mediator.Send(command);
            if (result is { success: false })
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Updates the categories of a post.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <param name="categoriesIds">List of category IDs.</param>
        /// <returns>200 if successful, 400 if failed.</returns>
        [HttpPut("{id}/categories")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy = "CanEditPost")]
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