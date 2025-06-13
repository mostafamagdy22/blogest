using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogest.domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
		public List<Post>? Posts { get; set; }
		public List<RefreshToken>? RefreshTokens { get; set; }
    }
}