using blogest.application.DTOs.responses;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class CommentsCommandRepository : ICommentsCommandRepository
{
    private readonly BlogCommandContext _context;
    private readonly IUsersRepository _usersRepository;
    public CommentsCommandRepository(BlogCommandContext context, IUsersRepository usersRepository)
    {
        _context = context;
        _usersRepository = usersRepository;
    }
    public async Task<CreateCommentResponse> CreateComment(Comment comment)
    {
        Guid userIdFromCookies = _usersRepository.GetUserIdFromCookies();

        if (comment.UserId != userIdFromCookies)
            throw new Exception("Some problem happend please try again later");

        
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return new CreateCommentResponse(comment.CommentId, comment.PostId, comment.UserId, "Comment created successfully", true);
    }

    public async Task<DeleteCommentResponse> DeleteComment(Guid commentId)
    {
        Guid userId = _usersRepository.GetUserIdFromCookies();
        Comment comment = await _context.Comments.FindAsync(commentId);

        if (comment.UserId != userId || comment == null)
        {
            throw new Exception("Some error happend in delete the comment");
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return new DeleteCommentResponse(Message: $"Comment {commentId} deleted",
        CommentId:commentId,IsSuccess:true);
    }

    public async Task<UpdateCommentResponse> UpdateComment(Guid commentId, string newContent)
    {
        Guid userId = _usersRepository.GetUserIdFromCookies();
        Comment comment = await _context.Comments.FindAsync(commentId);
        if (comment == null || userId != comment.UserId)
        {
            throw new Exception("Some error happend in update the comment");
        }
        comment.SetContent(newContent);
        await _context.SaveChangesAsync();

        return new UpdateCommentResponse(Message:$"Comment {commentId} updated successfully",IsSuccess:true,NewContent:comment.Content,CommentId:commentId);
    }
}