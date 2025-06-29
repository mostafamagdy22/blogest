namespace blogest.application.Validation.Comments;

public class DeleteCommentValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentValidator()
    {
        RuleFor(c => c.CommentId).NotEmpty().WithMessage("Comment id can't be empty");
    }
}