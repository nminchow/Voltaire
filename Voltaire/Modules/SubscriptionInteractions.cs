using System.Threading.Tasks;
using Discord.Interactions;
using Discord;

namespace Voltaire.Modules
{
    public class SubscriptionInteractions : InteractionsBase
    {

        public SubscriptionInteractions(DataBase database): base(database) {}


        [SlashCommand("pro", "generate a link to upgrade to voltaire pro or get current subscription info")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Pro()
        {
            await Controllers.Subscriptions.Pro.PerformAsync(new InteractionBasedContext(Context, PublicResponder), _database);
        }

        [SlashCommand("pro_cancel", "cancel your voltaire pro subscription")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Cancel()
        {
            await Controllers.Subscriptions.Cancel.PerformAsync(new InteractionBasedContext(Context, PublicResponder), _database);
        }
    }
}
