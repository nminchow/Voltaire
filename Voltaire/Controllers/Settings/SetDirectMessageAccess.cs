using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetDirectMessageAccess
    {
        public static async Task PerformAsync(ShardedCommandContext context, Boolean setting, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            guild.AllowDirectMessage = setting;
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"'Allow DM' set to {setting}");
        }
    }
}
