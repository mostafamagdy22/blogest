using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogest.domain.Entities
{
    public class PostCategory
    {
		public Guid PostId { get; set; }
		public Post Post { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
