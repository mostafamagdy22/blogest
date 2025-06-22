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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace blogest.infrastructure.Repositories
{
    public class PostsCommandRepository : IPostsCommandRepository
    {
        private readonly BlogCommandContext _commandContext;
        private readonly IUsersRepository _usersRepository;
        public PostsCommandRepository(BlogCommandContext commandContext, IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _commandContext = commandContext;
        }
        public async Task<Guid> AddAsync(Post post)
        {
            Guid postId = Guid.NewGuid();
            post.SetId(postId);

            Guid userId = _usersRepository.GetUserIdFromCookies();

            post.UserId = userId;

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

            Guid userId = _usersRepository.GetUserIdFromCookies();
            
            if (userId != post.UserId)
            {
                return new DeletePostResponse(IsSuccess: false, Message: $"Some Problem happend in delete post {postId}");
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