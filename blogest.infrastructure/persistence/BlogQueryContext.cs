namespace blogest.infrastructure.persistence
{
    public class BlogQueryContext : DbContext
    {
		public BlogQueryContext(DbContextOptions<BlogQueryContext> options) : base(options)
		{}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
