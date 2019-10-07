using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Models
{
    public class Guild
    {
        public Guild()
        {
            this.BannedIdentifiers = new HashSet<BannedIdentifier>();
        }

        public int ID { get; set; }
        public string DiscordId { get; set; }
        public string AllowedRole { get; set; }
        public string AdminRole { get; set; }
        public bool AllowDirectMessage { get; set; } = true;
        public bool UseUserIdentifiers { get; set; } = false;
        public int UserIdentifierSeed { get; set; } = 0;
        public virtual ICollection<BannedIdentifier> BannedIdentifiers { get; set; }
        public string SubscriptionId { get; set; }
        public int MessagesSentThisMonth { get; set; } = 0;
        public DateTime TrackingMonth { get; set; } = DateTime.Now;

    }
}
