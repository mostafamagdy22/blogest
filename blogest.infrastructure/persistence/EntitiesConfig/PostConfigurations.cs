﻿using blogest.infrastructure.Identity;

namespace blogest.infrastructure.persistence.EntitiesConfig
{
	public class PostConfigurations : IEntityTypeConfiguration<Post>
	{
		public void Configure(EntityTypeBuilder<Post> builder)
		{
			builder.HasKey(p => p.PostId);

			builder.Property(p => p.Title).
				IsRequired().
				HasColumnType("nvarchar(50)");

			builder.Property(p => p.Content)
				.IsRequired()
				.HasColumnType("nvarchar(max)");

			builder.Property(p => p.IsPublish)
				.IsRequired();

			builder.HasOne<AppUser>()
				.WithMany()
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
