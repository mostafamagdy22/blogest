namespace blogest.application.Validation.Comments;

public class UpdateCommentValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentValidator()
    {
        RuleFor(c => c.CommentId).NotEmpty().WithMessage("Comment id can't be empty");

        RuleFor(c => c.NewContent).NotEmpty().WithMessage("New Content can't be empty");
    }
}