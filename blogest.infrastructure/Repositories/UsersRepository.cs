using blogest.application.Interfaces.repositories;
using blogest.infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<AppUser> _userManger;
    public UsersRepository(UserManager<AppUser> userManager)
    {
        _userManger = userManager;
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