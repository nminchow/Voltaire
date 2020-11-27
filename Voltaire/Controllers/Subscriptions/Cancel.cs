using Discord.Commands;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Stripe;


namespace Voltaire.Controllers.Subscriptions
{
    class Cancel
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            if(EnsureActiveSubscription.Perform(guild, db))
            {
                var service = new SubscriptionService();
                var options = new SubscriptionCancelOptions {
                    Prorate = false
                };
                service.Cancel(guild.SubscriptionId, options);

                await context.Channel.SendMessageAsync(text: "Your subscription has been canceled. Use `!volt pro` to re-upgrade at any time!");
            }
            else
            {
                await context.Channel.SendMessageAsync(text: "You do not currently have an active Voltaire Pro subscription. To create one, use the" +
                    " `!volt pro` command.");
            }
        }
    }
}
