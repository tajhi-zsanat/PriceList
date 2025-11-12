using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }
        public bool Revoked { get; set; }

        // tie to device/browser via fingerprint if you like
        public string? UserAgent { get; set; }
        public string? Ip { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; } = default!;
    }
}
