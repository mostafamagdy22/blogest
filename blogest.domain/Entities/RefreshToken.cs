using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogest.domain.Entities
{
    public class RefreshToken
    {
		public Guid Id { get; set; }
		public string Token { get; set; }
		public Guid UserId { get; set; }
		public DateTime ExpiresAt { get; set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
		public DateTime? RevokedAt { get; private set; }
		public string? ReplacedByToken { get; private set; }
		public bool IsActive => RevokedAt == null && ExpiresAt > DateTime.UtcNow;
	
		public void Revoke(string? replaceByToken = null)
		{
			RevokedAt = DateTime.UtcNow;
			ReplacedByToken = replaceByToken;
		}
	}
}
