using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;
using Stripe;


namespace Voltaire.Controllers.Subscriptions
{
    class Pro
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            if(EnsureActiveSubscription.Perform(guild, db))
            {
                var service = new SubscriptionService();
                var subscription = service.Get(guild.SubscriptionId);
                var amount = subscription.Plan.Amount;
                var date = subscription.CurrentPeriodEnd;

                var message = $"Your current subscription will renew {date.Value.ToLongDateString()}.\n" +
                    $"To cancel your subscription, use the `/pro-cancel` command.";

                await Send.SendMessageToContext(context, message);
            }
            else
            {
                var size = context.Guild.MemberCount <= 200 ? "s" : "l";
                var url = $"https://nminchow.github.io/VoltaireWeb/upgrade?serverId={context.Guild.Id.ToString()}&type={size}";
                var view = Views.Info.Pro.Response(url, guild, db);
                try {
                    await Send.SendMessageToContext(context, view.Item1, embed: view.Item2);
                } catch (Discord.Net.HttpException e) {
                    await Send.SendMessageToContext(context, $"Use this URL to upgrade to Volatire Pro: {url}");
                }
            }
        }
    }
}
