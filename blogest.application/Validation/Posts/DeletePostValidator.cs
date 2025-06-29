namespace blogest.application.Validation.Posts;

public class DeletePostValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostValidator()
    {
        RuleFor(p => p.postId).NotEmpty().WithMessage("Post id can't be empty");
    }
}