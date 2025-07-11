using MediatR;

namespace blogest.application.Features.commands.Auth;

public record ForgetPasswordCommand(string Email) : IRequest<ForgetPasswordResponse>;