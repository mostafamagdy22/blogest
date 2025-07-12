using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogest.domain.Entities
{
	public class Post
	{
		public Guid PostId { get; private set; }
		public string Title { get; private set; }
		public string Content { get; private set; }
		public Guid UserId { get; set; } 
		public DateTime? PublishedAt { get; private set; }
		public bool IsPublish { get; set; }
		public List<Comment>? Comments { get; set; }
		public List<PostCategory> PostCategories { get; set; }
		public List<Like>? Likes { get; set; }
		public Post()
		{

		}
		public Post(Guid id, string title, string content, DateTime? publishedAt)
		{
			SetId(id);
			SetTitle(title);
			SetContent(content);
			SetPublishedAt(publishedAt);
		}
		public void SetId(Guid id)
		{
			if (PostId != Guid.Empty)
				throw new InvalidOperationException(blogest.domain.Constants.ErrorMessages.Conflict);
			if (id == Guid.Empty)
				throw new ArgumentException(blogest.domain.Constants.ErrorMessages.BadRequest, nameof(id));

			PostId = id;
		}

		public void SetTitle(string title)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Title cannot be null or empty.", nameof(title));
			if (title.Length > 100)
				throw new ArgumentException("Title cannot exceed 100 characters.", nameof(title));

			Title = title;
		}
		
		public void SetContent(string content)
		{
			if (string.IsNullOrWhiteSpace(content))
				throw new ArgumentException("Content cannot be null or empty.", nameof(content));
			if (content.Length < 10)
				throw new ArgumentException("Content must be at least 10 characters long.", nameof(content));
			if (content.Length > 5000)
				throw new ArgumentException("Content cannot exceed 5000 characters.", nameof(content));

			Content = content;
		}

		public void SetPublishedAt(DateTime? publishedAt)
		{
			if (publishedAt.HasValue && publishedAt.Value > DateTime.UtcNow)
				throw new ArgumentException("Published date cannot be in the future.", nameof(publishedAt));
			PublishedAt = publishedAt;
		}
	}
}
