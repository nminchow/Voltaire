using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class GenerateGuildUserIdentifierSeed
    {
        public static async Task PerformAsync(ShardedCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            guild.UserIdentifierSeed = new Random().Next(int.MinValue, int.MaxValue);
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: "User identifiers have been randomized.");
        }
    }
}
