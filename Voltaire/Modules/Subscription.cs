using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

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
