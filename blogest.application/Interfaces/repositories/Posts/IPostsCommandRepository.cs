namespace blogest.application.Interfaces.repositories.Posts;

public interface IPostsCommandRepository
{
    public Task<Guid> AddAsync(Post post);
    public Task<DeletePostResponse> DeletePost(Guid postId);
    public Task<UpdatePostResponse> UpdatePost(UpdatePostCommand updatePostCommand);
}
