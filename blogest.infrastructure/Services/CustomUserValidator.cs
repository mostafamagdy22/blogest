using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Services;

public class CustomUserValidator<TUser> : UserValidator<TUser> where TUser : class
{
    public CustomUserValidator(IdentityErrorDescriber errors = null) : base(errors) { }

    public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
    {
        var result = await base.ValidateAsync(manager, user);

        var errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

        return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }
}