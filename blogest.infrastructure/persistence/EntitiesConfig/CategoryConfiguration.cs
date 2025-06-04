namespace blogest.infrastructure.persistence.EntitiesConfig
{
	public class CategoryConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasKey(c => c.Id);

			builder.Property(c => c.Title)
				.IsRequired()
				.HasColumnType("nvarchar(50)");
		}
	}
}
