using blogest.application.Interfaces.services;
using MediatR;

namespace blogest.application.Features.handlers.Auth;

public class SignInHandler : IRequestHandler<SignInCommand, SignInResponse>
{
    private readonly IAuthService _authService;
    public SignInHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        SignInResponse response = await _authService.SignIn(request);
        return response;
    }
}