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
    public class Admin : ModuleBase<SocketCommandContext>
    {
        private DataBase _database;

        public Admin(DataBase database)
        {
            _database = database;
        }

        [Command("allow_dm", RunMode = RunMode.Async)]
        [Summary("Allow Direct Messages To Be Sent Annonymously Through This Server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AllowDj(Boolean allow)
        {
            await Controllers.Settings.SetDirectMessageAccess.PerformAsync(Context, allow, _database);
        }

        [Command("user_identifiers", RunMode = RunMode.Async)]
        [Summary("Use a Unique (Yet Annonymous) Identifier For Users When Sending Messages")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UserIdentifiers(Boolean allow)
        {
            await Controllers.Settings.SetUseUserIdentifiers.PerformAsync(Context, allow, _database);
        }

        [Command("new_identifiers", RunMode = RunMode.Async)]
        [Summary("Rotate User Identifiers")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UserIdentifiers()
        {
            await Controllers.Settings.GenerateGuildUserIdentifierSeed.PerformAsync(Context, _database);
        }
    }
}
