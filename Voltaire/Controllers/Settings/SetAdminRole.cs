using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetAdminRole
    {
        public static async Task PerformAsync(ShardedCommandContext context, SocketRole role, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            if (!EnsureActiveSubscription.Perform(guild, db))
            {
                await context.Channel.SendMessageAsync("You need an active Voltaire Pro subscription to set an admin role. To get started, use `!volt pro`");
                return;
            }

            guild.AdminRole = role.Id.ToString();
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"{role.Name} can now configure Voltaire and ban users on this server.");
        }
    }
}
