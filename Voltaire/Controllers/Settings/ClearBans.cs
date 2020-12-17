using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class ClearBans
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            db.RemoveRange(guild.BannedIdentifiers);
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: "Bans cleared");
        }
    }
}
