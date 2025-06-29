namespace blogest.application.Validation.Posts;

public class DeletePostByUserValidator : AbstractValidator<DeletePostsByUserCommand>
{
    public DeletePostByUserValidator()
    {
        RuleFor(p => p.userId).NotEmpty().WithMessage("User id can't be empty");
    }
}