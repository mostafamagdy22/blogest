namespace blogest.domain.Entities
{
    public class Category
    {
		public int Id { get; set; }
		public string Title { get; private set; }
		public List<PostCategory>? PostCategories { get; set; }
		public Category(){}
		public Category(string title)
		{
			SetTitle(title);
		}
		public void SetTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				throw new ArgumentException("Title cannot be empty!", nameof(title));

			Title = title;
		}
	}
}
