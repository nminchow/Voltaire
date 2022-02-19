using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;
using Voltaire.Models;

namespace Voltaire.Controllers.Settings
{
    class BanIdentifier
    {
        public static async Task PerformAsync(UnifiedContext context, string identifier, DataBase db)
        {
            if (!ValidIdentifier(identifier))
            {
                await Send.SendErrorWithDeleteReaction(context, "Please use the 4 digit number following the identifier to ban users.");
                return;
            }

            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            if(!EnsureActiveSubscription.Perform(guild,db))
            {
                await Send.SendErrorWithDeleteReaction(context,"You need an active Voltaire Pro subscription to ban users. To get started, use `!volt pro`");
                return;
            }

            if (guild.BannedIdentifiers.Any(x => x.Identifier == identifier))
            {
                await Send.SendErrorWithDeleteReaction(context,"That identifier is already banned!");
                return;
            }

            guild.BannedIdentifiers.Add(new BannedIdentifier { Identifier = identifier });
            db.SaveChanges();
            await Send.SendMessageToContext(context, $"{identifier} is now banned");
        }

        public static bool ValidIdentifier(string identifier)
        {
            return identifier.Length == 4 && int.TryParse(identifier, out int n);
        }
    }
}
