using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class ListBans
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            var bannedIdentifiers = guild.BannedIdentifiers.Select(x => x.Identifier).ToArray();

            if (bannedIdentifiers.Count() == 0)
            {
                await context.Channel.SendMessageAsync(text: $"No users are currently banned.");
                return;
            }

            await context.Channel.SendMessageAsync(text: $"**Banned Users:**\n{String.Join("\n", bannedIdentifiers)}");
        }
    }
}
