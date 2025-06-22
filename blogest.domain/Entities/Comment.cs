using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogest.domain.Entities
{
    public class Comment
    {
		public Guid CommentId { get; private set; }
		public Guid UserId { get; set; }
		public string Content { get; private set; }
		public DateTime PublishedAt { get; private set; }
		public Guid PostId { get; private set; }
		public Post Post { get; set; }
		public Comment()
		{
			PublishedAt = DateTime.UtcNow;
		}
		public Comment(Guid id,string content,Guid postId)
		{
			PublishedAt = DateTime.UtcNow;
			PostId = postId;
			SetId(id);
			SetContent(content);
		}

		public void SetId(Guid id)
		{
			if (CommentId != Guid.Empty)
				throw new InvalidOperationException("Comment ID has already been set.");
			if (id == Guid.Empty)
				throw new ArgumentException("Comment ID cannot be empty.", nameof(id));
			
			CommentId = id;
		}
		public void SetContent(string content)
		{
			if (string.IsNullOrEmpty(content))
				throw new ArgumentException("content cannot be empty!",nameof(content));
			if (content.Length > 300)
				throw new ArgumentException("content cannot exceed 300 characters!", nameof(content));
			Content = content;
		}

		
	}
}
