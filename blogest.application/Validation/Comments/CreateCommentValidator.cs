namespace blogest.application.Validation.Comments;

public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentValidator()
    {
        RuleFor(c => c.Content).NotEmpty().WithMessage("Comment Can't be empty!");

        RuleFor(c => c.UserId).NotEmpty().WithMessage("User id can't be empty");

        RuleFor(c => c.PostId).NotEmpty().WithMessage("Post id can't be empty");
    }
}