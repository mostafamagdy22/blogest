using FluentValidation;

namespace blogest.application.Validation;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(p => p.Title).MinimumLength(3).MaximumLength(30).NotNull().NotEmpty();

        RuleFor(p => p.Content).MinimumLength(20).MaximumLength(100).NotNull().NotEmpty();

        RuleFor(p => p.PublishDate).GreaterThanOrEqualTo(DateTime.UtcNow);
    }
}
