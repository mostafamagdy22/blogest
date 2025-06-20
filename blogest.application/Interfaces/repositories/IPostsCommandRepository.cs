using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.domain.Entities;

namespace blogest.application.Interfaces.repositories
{
    public interface IPostsCommandRepository
    {
        public Task<Guid> AddAsync(Post post);
        public Task<DeletePostResponse> DeletePost(Guid postId);
        public Task<UpdatePostResponse> UpdatePost(UpdatePostCommand updatePostCommand);
    }
}