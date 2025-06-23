using MediatR;

namespace blogest.application.Features.commands.Auth
{
    public record SignInCommand(string email,string password) : IRequest<SignInResponse>;
}