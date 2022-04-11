using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;

namespace Voltaire.Controllers.Settings
{
    class ClearBans
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            var guild = await FindOrCreateGuild.Perform(context.Guild, db);
            db.RemoveRange(guild.BannedIdentifiers);
            await db.SaveChangesAsync();
            await Send.SendMessageToContext(context, "Bans cleared");
        }
    }
}
