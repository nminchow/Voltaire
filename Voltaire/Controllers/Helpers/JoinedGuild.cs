using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Voltaire.Controllers.Helpers
{
    public static class JoinedGuild
    {
        public static async Task AnnoiceJoinChannel(SocketGuild guild)
        {
            var view = Views.Info.JoinedGuild.Response();
            await guild.TextChannels.First().SendMessageAsync(text: view.Item1, embed: view.Item2);
        }
    }
}
