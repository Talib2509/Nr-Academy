using NrAcademyCORE.Entities.Common;
using NrAcademyCORE.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class RefreshToken : BaseEntity
    {


        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        public string CreatedByIp { get; set; }

        public int AppUserId { get; set; }
        public AppUser User { get; set; }

        public string? ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
