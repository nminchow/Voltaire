using Discord.Commands;
using System;
using Voltaire.Controllers.Messages;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetEmbeds
    {
        public static async Task PerformAsync(UnifiedContext context, Boolean setting, DataBase db)
        {
            var guild = await FindOrCreateGuild.Perform(context.Guild, db);
            guild.UseEmbed = setting;
            await db.SaveChangesAsync();
            await Send.SendMessageToContext(context, $"'Embeds' set to {setting}");
        }
    }
}
