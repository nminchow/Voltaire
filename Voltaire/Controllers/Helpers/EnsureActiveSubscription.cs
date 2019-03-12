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
        public static bool Perform(Guild guild, SocketCommandContext context, DataBase db)
        {
            var service = new SubscriptionService();
            if (guild.SubscriptionId == null) {
                var response = service.List(new SubscriptionListOptions
                {
                    Limit = 100
                });
                var result = response.FirstOrDefault(x => x.Metadata.GetValueOrDefault("serverId") == guild.DiscordId) ?? null;
                if(result == null)
                {
                    context.Channel.SendMessageAsync("You need an active subscription to use this command. To get started, use `!volt upgrade`");
                    return false;
                }
                guild.SubscriptionId = result.Id;
                db.SaveChanges();
                
            } else
            {
                var subscription = service.Get(guild.SubscriptionId);
                if (subscription == null || subscription.CanceledAt != null)
                {
                    guild.SubscriptionId = null;
                    db.SaveChanges();
                    context.Channel.SendMessageAsync("You need an active subscription to use this command. To get started, use `!volt upgrade`");
                    return false;
                }
            }
            return true;
        }
    }
}
