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
        public async Task<Guid> AddAsync(Post post,List<int> categoryIds)
        {
            Guid postId = Guid.NewGuid();
            post.SetId(postId);

            Guid userId = _usersRepository.GetUserIdFromCookies();

            post.UserId = userId;

            List<PostCategory> postCategories = new List<PostCategory>();
            foreach (int categoryId in categoryIds)
            {
                postCategories.Add(new PostCategory(){CategoryId = categoryId,PostId = postId});
            }

            await _commandContext.Posts.AddAsync(post);
            await _commandContext.PostCategories.AddRangeAsync(postCategories);
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

        public async Task<DeletePostResponse> DeletePostsByUser(Guid userId)
        {
            Guid userIdFromCookies = _usersRepository.GetUserIdFromCookies();

            if (userIdFromCookies != userId)
            {
                return new DeletePostResponse(IsSuccess: false, Message: "You are not authorized to delete posts of this user.");
            }

            _commandContext.Posts.RemoveRange(_commandContext.Posts.Where(p => p.UserId == userId));
            await _commandContext.SaveChangesAsync();
            
            return new DeletePostResponse(IsSuccess: true, Message: $"All posts by user {userId} have been deleted.");
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

        public async Task<UpdatePostCategoriesResponse> updatePostCategories(UpdatePostCategoriesCommand command)
        {
            Post post = await _commandContext.Posts
            .Include(p => p.PostCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.PostId == command.postId);
            
            if (post == null)
                return new UpdatePostCategoriesResponse(IsSuccess: false, Message: "post not found");
            List<Category> categories = await _commandContext.Categories.
                                        Where(c => command.categoryIds.Contains(c.Id)).ToListAsync();

            if (categories.Count != command.categoryIds.Count)
                return new UpdatePostCategoriesResponse(IsSuccess:false,Message:"Some categories do not exist");

            post.PostCategories.Clear();
            foreach (Category category in categories)
            {
                post.PostCategories.Add(new PostCategory() {PostId = post.PostId,CategoryId = category.Id});
            }

            await _commandContext.SaveChangesAsync();
            return new UpdatePostCategoriesResponse(IsSuccess: true,Message:"categries updated successfully");
        }
    }
}