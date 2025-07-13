using Microsoft.AspNetCore.Identity;

namespace blogest.infrastructure.Identity
{
  public class AppUser : IdentityUser<Guid>
  {
    public string? Image { get; set; }
    public List<Post>? Posts { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }
    public ICollection<Save>? SavedPosts { get; set; }
    }
}