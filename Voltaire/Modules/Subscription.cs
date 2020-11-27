using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace Voltaire.Modules
{
    public class Subscription : ModuleBase<ShardedCommandContext>
    {
        private DataBase _database;

        public Subscription(DataBase database)
        {
            _database = database;
        }

        [Command("pro", RunMode = RunMode.Async)]
        [Summary("Upgrade your subscription")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Pro()
        {
            await Controllers.Subscriptions.Pro.PerformAsync(Context, _database);
        }

        [Command("cancel", RunMode = RunMode.Async)]
        [Summary("Cancel your subscription")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Cancel()
        {
            await Controllers.Subscriptions.Cancel.PerformAsync(Context, _database);
        }
    }
}
