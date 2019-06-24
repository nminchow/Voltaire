using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Models;

namespace Voltaire.Controllers.Settings
{
    class UnBanIdentifier
    {
        public static async Task PerformAsync(ShardedCommandContext context, string identifier, DataBase db)
        {
            if (!BanIdentifier.ValidIdentifier(identifier))
            {
                await context.Channel.SendMessageAsync("Please use the 4 digit number following the identifier to unban users.");
                return;
            }

            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            var identifiers = guild.BannedIdentifiers.Where(x => x.Identifier == identifier);
            if (identifiers.Count() == 0)
            {
                await context.Channel.SendMessageAsync("That user is not currently banned.");
                return;
            }

            db.BannedIdentifiers.RemoveRange(identifiers.ToList());

            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"{identifier} is now unbanned");
        }
    }
}
