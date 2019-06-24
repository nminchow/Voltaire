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
    class BanIdentifier
    {
        public static async Task PerformAsync(ShardedCommandContext context, string identifier, DataBase db)
        {
            if (!ValidIdentifier(identifier))
            {
                await context.Channel.SendMessageAsync("Please use the 4 digit number following the identifier to ban users.");
                return;
            }

            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            if(!EnsureActiveSubscription.Perform(guild,db))
            {
                await context.Channel.SendMessageAsync("You need an active Voltaire Pro subscription to ban users. To get started, use `!volt pro`");
                return;
            }

            if (guild.BannedIdentifiers.Any(x => x.Identifier == identifier))
            {
                await context.Channel.SendMessageAsync("That identifier is already banned!");
                return;
            }

            guild.BannedIdentifiers.Add(new BannedIdentifier { Identifier = identifier });
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"{identifier} is now banned");
        }

        public static bool ValidIdentifier(string identifier)
        {
            return identifier.Length == 4 && int.TryParse(identifier, out int n);
        }
    }
}
