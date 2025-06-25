using blogest.infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace blogest.infrastructure.persistence
{
    public class BlogCommandContext : IdentityDbContext<AppUser,IdentityRole<Guid>,Guid>
	{
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<PostCategory> PostCategories { get; set; }
		public BlogCommandContext(DbContextOptions<BlogCommandContext> options) : base(options)
		{}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new PostConfigurations());
			modelBuilder.ApplyConfiguration(new CommentConfiguration());
			modelBuilder.ApplyConfiguration(new PostCategoryConfiguration());
			modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryConfiguration());
		}
	}
}
