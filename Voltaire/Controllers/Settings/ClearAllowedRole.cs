using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class ClearAllowedRole
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            guild.AllowedRole = null;
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"All users can now use Voltaire.");
        }
    }
}
