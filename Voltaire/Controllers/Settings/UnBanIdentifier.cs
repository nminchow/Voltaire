using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;

namespace Voltaire.Controllers.Settings
{
    class UnBanIdentifier
    {
        public static async Task PerformAsync(UnifiedContext context, string identifier, DataBase db)
        {
            if (!BanIdentifier.ValidIdentifier(identifier))
            {
                await Send.SendErrorWithDeleteReaction(context, "Please use the 4 digit number following the identifier to unban users.");
                return;
            }

            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            var identifiers = guild.BannedIdentifiers.Where(x => x.Identifier == identifier);
            if (identifiers.Count() == 0)
            {
                await Send.SendErrorWithDeleteReaction(context, "That user is not currently banned.");
                return;
            }

            db.BannedIdentifiers.RemoveRange(identifiers.ToList());

            db.SaveChanges();
            await Send.SendMessageToContext(context, $"{identifier} is now unbanned");
        }
    }
}
