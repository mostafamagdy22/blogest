using blogest.infrastructure.Identity;

namespace blogest.infrastructure.persistence.EntitiesConfig
{
	public class CommentConfiguration : IEntityTypeConfiguration<Comment>
	{
		public void Configure(EntityTypeBuilder<Comment> builder)
		{
			builder.HasKey(c => c.CommentId);
			builder.Property(c => c.Content)
				.IsRequired()
				.HasColumnType("nvarchar(max)");
			builder.Property(c => c.PublishedAt)
				.IsRequired();

			builder.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(c => c.PostId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne<AppUser>()
				.WithMany(u => u.Comments)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.ClientCascade);
		}
	}
}
