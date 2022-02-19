using System;
using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;

namespace Voltaire.Controllers.Settings
{
    class ListBans
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            var bannedIdentifiers = guild.BannedIdentifiers.Select(x => x.Identifier).ToArray();

            if (bannedIdentifiers.Count() == 0)
            {
                await Send.SendMessageToContext(context, $"No users are currently banned.");
                return;
            }

            await Send.SendMessageToContext(context, $"**Banned Users:**\n{String.Join("\n", bannedIdentifiers)}");
        }
    }
}
