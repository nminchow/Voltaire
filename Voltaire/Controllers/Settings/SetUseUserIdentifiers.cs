using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetUseUserIdentifiers
    {
        public static async Task PerformAsync(ShardedCommandContext context, Boolean setting, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            guild.UseUserIdentifiers = setting;
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"'User Identifiers' set to {setting}");
        }
    }
}
