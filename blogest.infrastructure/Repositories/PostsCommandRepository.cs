using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
namespace blogest.infrastructure.Repositories
{
    public class PostsCommandRepository : IPostsCommandRepository
    {
        private readonly BlogCommandContext _commandContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostsCommandRepository(BlogCommandContext commandContext, IHttpContextAccessor httpContextAccessor)
        {
            _commandContext = commandContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Guid> AddAsync(Post post)
        {
            Guid postId = Guid.NewGuid();
            post.SetId(postId);

            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

            if (token == null)
                throw new ValidationException("UnAuthoraized");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdCliam = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var userId = userIdCliam?.Value;

            post.UserId = Guid.Parse(userId);

            await _commandContext.Posts.AddAsync(post);
            await _commandContext.SaveChangesAsync();
            return postId;
        }

        public async Task<DeletePostResponse> DeletePost(Guid postId)
        {
            Post post = await _commandContext.Posts.FindAsync(postId);
            if (post == null)
            {
                return new DeletePostResponse(IsSuccess: false,
                Message: "Post not found");
            }
            _commandContext.Posts.Remove(post);
            await _commandContext.SaveChangesAsync();

            return new DeletePostResponse(IsSuccess:true,Message:$"Post with ID {postId} has been deleted.");
        }

        public async Task<UpdatePostResponse> UpdatePost(UpdatePostCommand updatePostCommand)
        {
            Post post = await _commandContext.Posts.FindAsync(updatePostCommand.postId);
            if (post == null)
                return new UpdatePostResponse(null, null, null, "Post Not Found!");

            post.SetTitle(updatePostCommand.Title);
            post.SetContent(updatePostCommand.Content);
            await _commandContext.SaveChangesAsync();
            return new UpdatePostResponse(post.Title,post.Content,DateTime.UtcNow,$"post {updatePostCommand.postId} updated successfully!");
        }
    }
}