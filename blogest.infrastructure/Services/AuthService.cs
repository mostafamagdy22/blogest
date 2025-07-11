using AutoMapper;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Web;
using Microsoft.Extensions.Configuration;
using blogest.domain.Constants;
namespace blogest.infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersRepository _useresRepository;
        private readonly BlogCommandContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AuthService(IEmailService emailService, IConfiguration configuration, BlogCommandContext blogCommandContext, IUsersRepository usersRepository, IMapper mapper, UserManager<AppUser> userManager, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
            _configuration = configuration;
            _context = blogCommandContext;
            _useresRepository = usersRepository;
            _mapper = mapper;
            _userManager = userManager;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SignUpResponseDto> SignUp(User user)
        {
            user.Id = Guid.NewGuid();
            AppUser appUser = _mapper.Map<AppUser>(user);
            IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);

            string accessToken = _jwtService.GenrateToken(appUser.Id, appUser.UserName, new List<string> { "User" });
            string refreshToken = _jwtService.GenerateRefreshToken();

            HttpContext context = _httpContextAccessor.HttpContext;
            context.Response.Cookies.Append("jwt", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(1)
            });

            return new SignUpResponseDto
            {
                SignUpSuccessfully = result.Succeeded ? true : false,
                UserId = result.Succeeded ? appUser.Id : Guid.Empty,
                Message = result.Succeeded
        ? "Sign up completed successfully"
        : string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
        public async Task<SignInResponse> SignIn(SignInCommand user)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(user.email);
            if (appUser == null)
            {
                string fakeHashedPassword = "$2a$11$XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
                BCrypt.Net.BCrypt.Verify(user.password, fakeHashedPassword);

                return new SignInResponse("Invalid credentials", false, null, null);
            }
            
            bool isPasswordValid = await _userManager.CheckPasswordAsync(appUser, user.password);
            if (!isPasswordValid) return new SignInResponse("Invalid credentials", false, null, null);

            IList<string> roles = await _userManager.GetRolesAsync(appUser);

            string accessToken = _jwtService.GenrateToken(appUser.Id, appUser.UserName, roles);
            string refreshToken = _jwtService.GenerateRefreshToken();
            var context = _httpContextAccessor.HttpContext;
            context.Response.Cookies.Append("jwt", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });
            context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            await _jwtService.AddRefreshTokenToDb(refreshToken, appUser.Id);
            return new SignInResponse("Sign in successfully", true, accessToken, refreshToken);
        }

        public async Task<string> LogOut()
        {
            Guid? userId = _useresRepository.GetUserIdFromCookies();

            List<RefreshToken> refreshTokens = await _context.RefreshTokens.Where(
                rt => rt.ExpiresAt > DateTime.UtcNow
                 && rt.RevokedAt == null
                 && rt.UserId == userId).ToListAsync();

            refreshTokens.ForEach(rt => rt.Revoke());
            await _context.SaveChangesAsync();

            HttpContext context = _httpContextAccessor.HttpContext;

            context.Response.Cookies.Delete("jwt");
            context.Response.Cookies.Delete("refreshToken");


            await context.SignOutAsync();


            return $"user ${userId} log out Successfully";
        }

        public async Task<User> CreateUserFromGoogleAsync(string email, string name, string googleId)
        {
            AppUser newUser = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                EmailConfirmed = true,
                UserName = name,
            };

         
            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            UserLoginInfo info = new UserLoginInfo(
                loginProvider: "Google",
                providerKey: googleId,
                displayName: "Google"
            );
            var loginResult = await _userManager.AddLoginAsync(newUser,info);
            if (!loginResult.Succeeded)
                throw new Exception("Login link failed: " + string.Join(", ",loginResult.Errors.Select(e => e.Description)));
            User user = _mapper.Map<User>(newUser);

            return user;
        }

        public async Task<ForgetPasswordResponse> ForgetPassword(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ForgetPasswordResponse
                {
                    IsSuccess = false,
                    Message = "No user found with this email"
                };

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string encodedToken = HttpUtility.UrlEncode(token);

            string resetUrl = $"{_configuration["App:ClientBaseUrl"]}/reset-password?token={encodedToken}&email={email}";
            string body = string.Format(EmailMessages.ResetPasswordBodyTemplate, resetUrl);

            await _emailService.SendEmailAsync(email, EmailMessages.ResetPasswordSubject, body);

            return new ForgetPasswordResponse { IsSuccess = true, Message = "Reset password email sent successfully" };
        }

        public async Task<ForgetPasswordCallBackResponse> ForgetPasswordCallBack(string email, string token, string password)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new ForgetPasswordCallBackResponse
                {
                    IsSuccess = false,
                    isAuth = false,
                    Message = "User not found"
                };

            string decodedToken = HttpUtility.UrlDecode(token);
            bool isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", decodedToken);
            if (!isValid) return new ForgetPasswordCallBackResponse
            {
                IsSuccess = false,
                isAuth = false,
                Message = "Bad Request"
            };

            IdentityResult result = await _userManager.ResetPasswordAsync(user,decodedToken,password);

            if (!result.Succeeded)
                return new ForgetPasswordCallBackResponse
                {
                    IsSuccess = false,
                    isAuth = false,
                    Message = "BadRequest"
                };

            SignInResponse signInResult =  await SignIn(new SignInCommand(user.Email,password));

            if (!signInResult.isAuth)
                return new ForgetPasswordCallBackResponse
                {
                    IsSuccess = false,
                    isAuth = false,
                    Message = "Bad Request"
                };

            return new ForgetPasswordCallBackResponse
                {
                    IsSuccess = true,
                    isAuth = true,
                    Message = "Reset password done successfully"
                };
        }
    }
}