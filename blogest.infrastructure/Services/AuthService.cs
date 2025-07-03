using AutoMapper;
using blogest.application.Interfaces.services;
using blogest.infrastructure.Identity;
using blogest.infrastructure.persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
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
        public AuthService(BlogCommandContext blogCommandContext, IUsersRepository usersRepository, IMapper mapper, UserManager<AppUser> userManager, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
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
    }
}