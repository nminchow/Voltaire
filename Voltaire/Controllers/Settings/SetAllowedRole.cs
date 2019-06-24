using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetAllowedRole
    {
        public static async Task PerformAsync(ShardedCommandContext context, SocketRole role, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            guild.AllowedRole = role.Id.ToString();
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"{role.Name} is now the only role that can use Voltaire on this server");
        }
    }
}
