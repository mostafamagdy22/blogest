namespace blogest.application.Interfaces.repositories.Users;

public interface IUsersRepository
{
    /// <summary>
    /// Checks if the given email exists in the system.
    /// </summary>
    /// <param name="email">The email to check.</param>
    /// <returns>True if the email exists; otherwise, false.</returns>
    public Task<bool> IsEmailExit(string email);

    /// <summary>
    /// Checks if the given username exists in the system.
    /// </summary>
    /// <param name="userName">The username to check.</param>
    /// <returns>True if the username exists; otherwise, false.</returns>
    public Task<bool> IsUserNameExit(string userName);

    /// <summary>
    /// Gets the user ID from cookies.
    /// </summary>
    /// <returns>The user ID extracted from cookies.</returns>
    public Guid? GetUserIdFromCookies();
    public Task<User> GetUserByEmailAsync(string email);
}