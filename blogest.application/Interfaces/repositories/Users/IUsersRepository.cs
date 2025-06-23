namespace blogest.application.Interfaces.repositories.Users;

public interface IUsersRepository
{
    public Task<bool> IsEmailExit(string email);
    public Task<bool> IsUserNameExit(string userName);
    public Guid GetUserIdFromCookies();
}