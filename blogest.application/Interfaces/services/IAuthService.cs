namespace blogest.application.Interfaces.services
{
    public interface IAuthService
    {
        public Task<SignUpResponseDto> SignUp(User signUpCommand);
        public Task<SignInResponse> SignIn(SignInCommand user);
        public Task<string> LogOut();
        public Task<User> CreateUserFromGoogleAsync(string email,string name,string googleId);
    }
}