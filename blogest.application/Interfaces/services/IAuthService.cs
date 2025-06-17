using blogest.application.DTOs.responses;
using blogest.application.Features.commands;
using blogest.domain.Entities;

namespace blogest.application.Interfaces.services
{
    public interface IAuthService
    {
        public Task<SignUpResponseDto> SignUp(User signUpCommand);
        public Task<SignInResponse> SignIn(SignInCommand user);
    }
}