using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;
using Stripe;


namespace Voltaire.Controllers.Subscriptions
{
    class Cancel
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            var guild = await FindOrCreateGuild.Perform(context.Guild, db);
            if(EnsureActiveSubscription.Perform(guild, db))
            {
                var service = new SubscriptionService();
                var options = new SubscriptionCancelOptions {
                    Prorate = false
                };
                service.Cancel(guild.SubscriptionId, options);

                await Send.SendMessageToContext(context, "Your subscription has been canceled. Use `/pro` to re-upgrade at any time!");
            }
            else
            {
                await Send.SendMessageToContext(context, "You do not currently have an active Voltaire Pro subscription. To create one, use the" +
                    " `/pro` command.");
            }
        }
    }
}
