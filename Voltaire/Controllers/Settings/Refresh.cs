using Discord.Commands;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Settings
{
    class Refresh
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            Helpers.JoinedGuild.Refresh(context.Guild);

            await context.Channel.SendMessageAsync(text: "User list has been refreshed.");
        }
    }
}
