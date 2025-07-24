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
        Guid? userIdFromCookies = _usersRepository.GetUserIdFromCookies();

        if (comment.UserId != userIdFromCookies)
            return new CreateCommentResponse(comment.CommentId, comment.PostId, comment.UserId, blogest.domain.Constants.ErrorMessages.Unauthorized, false);

        await _context.Comments.AddAsync(comment);
        var result = await _context.SaveChangesAsync();
        if (result < 1)
            return new CreateCommentResponse(comment.CommentId, comment.PostId, comment.UserId, blogest.domain.Constants.ErrorMessages.InternalServerError, false);

        return new CreateCommentResponse(comment.CommentId, comment.PostId, comment.UserId, "Comment created successfully", true);
    }

    public async Task<DeleteCommentResponse> DeleteComment(Guid commentId)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies();
        Comment? comment = await _context.Comments.FindAsync(commentId);

        if (comment == null)
            return new DeleteCommentResponse(Message: blogest.domain.Constants.ErrorMessages.NotFound, CommentId: commentId, IsSuccess: false);
        if (comment.UserId != userId)
            return new DeleteCommentResponse(Message: blogest.domain.Constants.ErrorMessages.Unauthorized, CommentId: commentId, IsSuccess: false);

        _context.Comments.Remove(comment);
        var result = await _context.SaveChangesAsync();
        if (result < 1)
            return new DeleteCommentResponse(Message: blogest.domain.Constants.ErrorMessages.InternalServerError, CommentId: commentId, IsSuccess: false);

        return new DeleteCommentResponse(Message: $"Comment {commentId} deleted", CommentId: commentId, IsSuccess: true);
    }

    public async Task<UpdateCommentResponse> UpdateComment(Guid commentId, string newContent)
    {
        Guid? userId = _usersRepository.GetUserIdFromCookies();
        Comment? comment = await _context.Comments.FindAsync(commentId);
        if (comment == null)
            return new UpdateCommentResponse(Message: blogest.domain.Constants.ErrorMessages.NotFound, IsSuccess: false, NewContent: null, CommentId: commentId);
        if (userId != comment.UserId)
            return new UpdateCommentResponse(Message: blogest.domain.Constants.ErrorMessages.Unauthorized, IsSuccess: false, NewContent: null, CommentId: commentId);

        comment.SetContent(newContent);
        var result = await _context.SaveChangesAsync();
        if (result < 1)
            return new UpdateCommentResponse(Message: blogest.domain.Constants.ErrorMessages.InternalServerError, IsSuccess: false, NewContent: null, CommentId: commentId);

        return new UpdateCommentResponse(Message: $"Comment {commentId} updated successfully", IsSuccess: true, NewContent: comment.Content, CommentId: commentId);
    }
}