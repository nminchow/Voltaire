using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voltaire.Models;
using Stripe;

namespace Voltaire.Controllers.Helpers
{
    class EnsureActiveSubscription
    {
        public static bool Perform(Guild guild, DataBase db)
        {
            if (guild.DiscordId == "426894892262752256")
            {
                return true;
            }

            if (guild.SubscriptionId != null)
            {
                var service = new SubscriptionService();
                try
                {
                    var subscription = service.Get(guild.SubscriptionId);
                    if (subscription != null && subscription.CanceledAt == null)
                    {
                        return true;
                    }
                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                guild.SubscriptionId = null;
                db.SaveChanges();
            }
            return QueryForSubscription(guild, db);
        }

        private static bool QueryForSubscription(Guild guild, DataBase db)
        {
            var service = new SubscriptionService();
            var response = service.List(new SubscriptionListOptions
            {
                Limit = 100
            });
            var result = response.FirstOrDefault(x => x.Metadata.GetValueOrDefault("serverId") == guild.DiscordId) ?? null;
            if (result == null)
            {
                return false;
            }
            guild.SubscriptionId = result.Id;
            db.SaveChanges();
            return true;
        }
    }
}
