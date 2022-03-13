using Discord.WebSocket;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;


namespace Voltaire.Controllers.Settings
{
    class SetAdminRole
    {
        public static async Task PerformAsync(UnifiedContext context, SocketRole role, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            if (!EnsureActiveSubscription.Perform(guild, db))
            {
                await Send.SendMessageToContext(context, "You need an active Voltaire Pro subscription to set an admin role. To get started, use `/pro`");
                return;
            }

            guild.AdminRole = role.Id.ToString();
            db.SaveChanges();
            await Send.SendMessageToContext(context, $"{role.Name} can now configure Voltaire and ban users on this server.");
        }
    }
}
