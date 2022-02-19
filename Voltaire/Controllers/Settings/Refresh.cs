using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Controllers.Settings
{
    class Refresh
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            Helpers.JoinedGuild.Refresh(context.Guild);

            await Send.SendMessageToContext(context, "User list has been refreshed.");
        }
    }
}
