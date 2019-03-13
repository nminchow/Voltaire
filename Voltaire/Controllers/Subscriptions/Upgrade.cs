using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;
using Voltaire.Models;

namespace Voltaire.Controllers.Subscriptions
{
    class Upgrade
    {
        public static async Task PerformAsync(SocketCommandContext context, DataBase db)
        {
            var guild = FindOrCreateGuild.Perform(context.Guild, db);
            if(EnsureActiveSubscription.Perform(guild, db))
            {
                // todo info about subscription or just cancel message info
            }
            else
            {
                var size = context.Guild.MemberCount <= 200 ? "s" : "l";
                var url = $"https://nminchow.github.io/VoltaireWeb/upgrade?serverId={context.Guild.Id.ToString()}&type={size}";
                await context.Channel.SendMessageAsync(text: $"Use this URL to upgrade to Volatire Pro: {url}");
            }
        }
    }
}
