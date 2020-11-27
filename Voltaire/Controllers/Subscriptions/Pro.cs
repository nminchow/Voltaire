using Discord.Commands;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Stripe;


namespace Voltaire.Controllers.Subscriptions
{
    class Pro
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            if(EnsureActiveSubscription.Perform(guild, db))
            {
                var service = new SubscriptionService();
                var subscription = service.Get(guild.SubscriptionId);
                var amount = subscription.Plan.Amount;
                var date = subscription.CurrentPeriodEnd;

                var message = $"Your current subscription will renew {date.Value.ToLongDateString()}.\n" +
                    $"To cancel your subscription, use the `!volt cancel` command.";

                await context.Channel.SendMessageAsync(text: message);
            }
            else
            {
                var size = context.Guild.MemberCount <= 200 ? "s" : "l";
                var url = $"https://nminchow.github.io/VoltaireWeb/upgrade?serverId={context.Guild.Id.ToString()}&type={size}";
                await context.Channel.SendMessageAsync(text: $"Use this URL to upgrade to Volatire Pro: {url}");
            }
        }
    }
}
