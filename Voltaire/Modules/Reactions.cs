using System.Threading.Tasks;
using Discord.Commands;

namespace Voltaire.Modules
{
    public class Reaction : ModuleBase<ShardedCommandContext>
    {
        private DataBase _database;

        public Reaction(DataBase database)
        {
            _database = database;
        }

        [Command("react", RunMode = RunMode.Async)]
        [Summary("add a reaction to a message")]
        public async Task React(ulong messageId, string emoji)
        {
            await Controllers.Reactions.React.PerformAsync(Context, messageId, emoji, _database);
        }

        [Command("list_emotes", RunMode = RunMode.Async)]
        [Summary("list custom emoji codes")]
        public async Task List()
        {
            await Controllers.Subscriptions.Cancel.PerformAsync(Context, _database);
        }
    }
}
