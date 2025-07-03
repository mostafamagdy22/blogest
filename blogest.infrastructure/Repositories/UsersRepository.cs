using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using blogest.application.Interfaces.repositories;
using blogest.infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<AppUser> _userManger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    public UsersRepository(IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userManger = userManager;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        AppUser user = await _userManger.FindByEmailAsync(email);

        if (user == null)
            return null;

        User userDto = _mapper.Map<User>(user);
        return userDto;
    }

    public Guid? GetUserIdFromCookies()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null || !Guid.TryParse(claim.Value, out Guid userId))
            return null;

        return userId;
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