namespace blogest.application.Interfaces.services
{
    public interface IAuthService
    {
        public Task<SignUpResponseDto> SignUp(User signUpCommand);
        public Task<SignInResponse> SignIn(SignInCommand user);
        public Task<string> LogOut();
        public Task<User> CreateUserFromGoogleAsync(string email, string name, string googleId);
        public Task<ForgetPasswordResponse> ForgetPassword(string email);
        public Task<ForgetPasswordCallBackResponse> ForgetPasswordCallBack(string email,string token,string password);
    }
}