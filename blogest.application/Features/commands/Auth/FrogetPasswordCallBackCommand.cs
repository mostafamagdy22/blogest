using MediatR;

namespace blogest.application.Features.commands.Auth;

public class FrogetPasswordCallBackCommand : IRequest<ForgetPasswordCallBackResponse>
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}