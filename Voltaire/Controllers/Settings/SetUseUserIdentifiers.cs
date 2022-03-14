using System;
using Voltaire.Controllers.Messages;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetUseUserIdentifiers
    {
        public static async Task PerformAsync(UnifiedContext context, Boolean setting, DataBase db)
        {
            var guild = await FindOrCreateGuild.Perform(context.Guild, db);
            guild.UseUserIdentifiers = setting;
            await db.SaveChangesAsync();
            await Send.SendMessageToContext(context, $"'User Identifiers' set to {setting}");
        }
    }
}
