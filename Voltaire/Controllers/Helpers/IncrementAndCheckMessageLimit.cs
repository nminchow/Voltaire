using System;
using Voltaire.Models;

namespace Voltaire.Controllers.Helpers
{
    class IncrementAndCheckMessageLimit
    {
        public static bool Perform(Guild guild, DataBase db)
        {
            // see if we need to reset the counter
            if (guild.TrackingMonth.Month != DateTime.Now.Month)
            {
                guild.TrackingMonth = DateTime.Now;
                guild.MessagesSentThisMonth = 0;
            }

            // increment counter by one
            guild.MessagesSentThisMonth += 1;
            db.SaveChanges();

            return guild.MessagesSentThisMonth <= 50 || EnsureActiveSubscription.Perform(guild, db);
        }
    }
}
