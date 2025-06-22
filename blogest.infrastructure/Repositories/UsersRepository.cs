using System.IdentityModel.Tokens.Jwt;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<AppUser> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
    public UsersRepository(UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManger = userManager;
    }

    public Guid GetUserIdFromCookies()
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdCliam = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        var userId = userIdCliam?.Value;
        return Guid.Parse(userId);
    }

    public async Task<bool> IsEmailExit(string email)
    {
        AppUser appUser = await _userManger.FindByEmailAsync(email);
        if (appUser != null)
            return true;
        return false;
    }

    public async Task<bool> IsUserNameExit(string userName)
    {
        if (await _userManger.FindByNameAsync(userName) != null)
            return true;
        return false;
    }
}