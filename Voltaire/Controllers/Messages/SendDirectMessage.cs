using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class SendDirectMessage
    {
        public static async Task PerformAsync(SocketCommandContext currentContext, string userName, string message)
        {
            userName = userName.StartsWith('@') ? userName.Substring(1) : userName;

            var user = currentContext.Client.Guilds
                .Where(x => x.Users.ToLookup(u => u.Id)[currentContext.User.Id] != null)
                .Aggregate(new List<SocketGuildUser>(), (acc, item) => acc.Concat(item.Users).ToList())
                .FirstOrDefault(x => x.Username.ToLower() == userName.ToLower() && !x.IsBot);

            if (user == null)
            {
                await currentContext.Channel.SendMessageAsync("user not found");
                return;
            }

            var userChannel = await user.GetOrCreateDMChannelAsync();
            await userChannel.SendMessageAsync($"an anonymous user says: {message}");
            await currentContext.Channel.SendMessageAsync("Sent!");
        }
    }
}
