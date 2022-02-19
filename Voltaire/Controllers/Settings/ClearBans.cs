using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;

namespace Voltaire.Controllers.Settings
{
    class ClearBans
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            db.RemoveRange(guild.BannedIdentifiers);
            db.SaveChanges();
            await Send.SendMessageToContext(context, "Bans cleared");
        }
    }
}
