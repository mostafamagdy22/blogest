using blogest.application.Interfaces.services;
using MediatR;

namespace blogest.application.Features.handlers.Auth;

public class ForgetPasswordCallBackHandler : IRequestHandler<FrogetPasswordCallBackCommand, ForgetPasswordCallBackResponse>
{
    private readonly IAuthService _authService;
    public ForgetPasswordCallBackHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<ForgetPasswordCallBackResponse> Handle(FrogetPasswordCallBackCommand request, CancellationToken cancellationToken)
    {
        ForgetPasswordCallBackResponse response = await _authService.ForgetPasswordCallBack(request.Email, request.Token, request.NewPassword);

        return response;
    }
}