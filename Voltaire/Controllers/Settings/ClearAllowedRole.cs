using System.Threading.Tasks;
using Voltaire.Controllers.Messages;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class ClearAllowedRole
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            var guild = await FindOrCreateGuild.Perform(context.Guild, db);
            guild.AllowedRole = null;
            await db.SaveChangesAsync();
            await Send.SendMessageToContext(context, $"All users can now use Voltaire.");
        }
    }
}
