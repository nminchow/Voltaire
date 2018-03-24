using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Voltaire.Controllers.Helpers
{
    public static class UserJoined
    {
        public static async Task SendJoinedMessage(SocketGuildUser user)
        { 
            if (user.IsBot)
                return;
            var view = Views.Info.UserJoined.Response();
            var channel = await user.GetOrCreateDMChannelAsync();
            await channel.SendMessageAsync(text: view.Item1, embed: view.Item2);
        }
    }
}
