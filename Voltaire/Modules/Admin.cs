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
        [Summary("Allow Direct Messages To Be Sent Annonymously Through This Guild")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetDjAsync(Boolean allow)
        {
            await Controllers.Settings.SetDirectMessageAccess.PerformAsync(Context, allow, _database);
        }
    }
}
