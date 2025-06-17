using blogest.infrastructure.Identity;

namespace blogest.infrastructure.persistence.EntitiesConfig
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasKey(rt => rt.Id);

			builder.HasOne<AppUser>()
				.WithMany(u => u.RefreshTokens)
				.HasForeignKey(rt => rt.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(rt => rt.Token)
				.IsRequired();

			builder.Property(rt => rt.ExpiresAt)
				.IsRequired();

			builder.Property(rt => rt.CreatedAt)
				.IsRequired();

		}
	}
}
