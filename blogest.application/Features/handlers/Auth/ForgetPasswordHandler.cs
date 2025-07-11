using blogest.application.Interfaces.services;
using MediatR;

namespace blogest.application.Features.handlers.Auth;

public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, ForgetPasswordResponse>
{
    private readonly IAuthService _authService;
    public ForgetPasswordHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<ForgetPasswordResponse> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        ForgetPasswordResponse response = await _authService.ForgetPassword(request.Email);

        return response;
    }
}