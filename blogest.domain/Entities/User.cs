using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogest.domain.Entities
{
    public class User
    {
		public string? Image { get; set; }
		public List<Post>? Posts { get; set; }
		public List<RefreshToken>? RefreshTokens { get; set; }
	}
}
