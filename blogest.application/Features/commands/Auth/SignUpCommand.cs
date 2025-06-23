using MediatR;

namespace blogest.application.Features.commands.Auth;

public record SignUpCommand : IRequest<SignUpResponseDto>
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
