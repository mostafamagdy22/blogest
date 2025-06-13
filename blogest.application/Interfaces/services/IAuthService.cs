using blogest.application.DTOs.responses;
using blogest.domain.Entities;

namespace blogest.application.Interfaces.services
{
    public interface IAuthService
    {
        public Task<SignUpResponseDto> SignUp(User signUpCommand);
    }
}