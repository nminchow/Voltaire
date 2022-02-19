using System;
using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Controllers.Messages;
using Voltaire.Models;

namespace Voltaire.Controllers.Settings
{
    class GenerateGuildUserIdentifierSeed
    {
        public static async Task PerformAsync(UnifiedContext context, DataBase db)
        {
            try
            {
                var guild = FindOrCreateGuild.Perform(context.Guild, db);

                // toList to force enumeration before we shuffle identifier
                var bannedUsers = context.Guild.Users.Where(x => PrefixHelper.UserBlocked(x.Id, guild)).ToList();

                guild.UserIdentifierSeed = new Random().Next(int.MinValue, int.MaxValue);

                var items = bannedUsers.Select(x => PrefixHelper.GetIdentifierString(x.Id, guild)).Select(x => new BannedIdentifier { Identifier = x });

                db.RemoveRange(guild.BannedIdentifiers);

                items.Select((x) => {
                    guild.BannedIdentifiers.Add(x);
                    return true;
                }).ToList();

                db.SaveChanges();

                await Send.SendMessageToContext(context, "User identifiers have been randomized.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error rotating");
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
