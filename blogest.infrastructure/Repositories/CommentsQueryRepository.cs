using blogest.application.DTOs.responses;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.persistence;

namespace blogest.infrastructure.Repositories;

public class CommentsQueryRepository : ICommentsQueryRepository
{
    private readonly BlogCommandContext _context;
    public CommentsQueryRepository(BlogCommandContext context)
    {
        _context = context;
    }
    public async Task<GetCommentByIdResponse> GetCommentById(Guid commentId)
    {
        var comment1 = await _context.Comments.Join(_context.DomainUsers,
        comment => comment.UserId,
        user => user.Id,
        (comment, user) => new { Comment = comment, User = user }).ToListAsync();
        var comment = comment1.FirstOrDefault(c => c.Comment.CommentId == commentId);
        if (comment == null)
            throw new Exception("No comment by this id!");


        return new GetCommentByIdResponse(
            content: comment.Comment.Content,
            userName: comment.User.UserName,
            postId: comment.Comment.PostId
        );
    }
}