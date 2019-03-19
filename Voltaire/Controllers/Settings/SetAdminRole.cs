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
        public static async Task PerformAsync(SocketCommandContext context, SocketRole role, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            guild.AdminRole = role.Id.ToString();
            db.SaveChanges();
            await context.Channel.SendMessageAsync(text: $"{role.Name} can now configure Voltaire and ban users on this server.");
        }
    }
}
