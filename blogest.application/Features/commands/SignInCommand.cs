using blogest.application.DTOs.responses;
using MediatR;

namespace blogest.application.Features.commands
{
    public record SignInCommand(string email,string password) : IRequest<SignInResponse>;
}