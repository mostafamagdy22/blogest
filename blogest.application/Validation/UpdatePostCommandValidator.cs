using blogest.application.Features.commands;
using blogest.application.Interfaces.repositories;
using FluentValidation;

namespace blogest.application.Validation;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    private readonly IPostsQueryRepository _postsQueryRepository;
    public UpdatePostCommandValidator(IPostsQueryRepository postsQueryRepository)
    {
        _postsQueryRepository = postsQueryRepository;
        RuleFor(p => p.Title).MinimumLength(3).MaximumLength(30).NotNull().NotEmpty();

        RuleFor(p => p.Content).MinimumLength(20).MaximumLength(100).NotNull().NotEmpty();

        RuleFor(p => p.postId)
        .MustAsync(PostExists).WithMessage("Post with the give id doesnt exit");
    }
    private async Task<bool> PostExists(Guid postId,CancellationToken cancellationToken)
    {
        return await _postsQueryRepository.ExistsAsync(postId);
    }
}