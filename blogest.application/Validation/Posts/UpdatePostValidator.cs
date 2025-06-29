namespace blogest.application.Validation.Posts;

public class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostValidator()
    {
        RuleFor(p => p.Content).NotEmpty().WithMessage("New content can't be empty");

        RuleFor(p => p.postId).NotEmpty().WithMessage("Post id can't be empty");

        RuleFor(p => p.Title).NotEmpty().WithMessage("New Title can't be empty");
    }
}