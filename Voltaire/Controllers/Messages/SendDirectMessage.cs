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

            Console.WriteLine("Starting DM Query");
            var guildList = currentContext.Client.Guilds.Where(x => x.Users.ToLookup(u => u.Id)[currentContext.User.Id] != null);
            Console.WriteLine("Got guilds");
            var userList = guildList.Aggregate(new List<SocketGuildUser>(), (acc, item) => acc.Concat(item.Users).ToList());
            Console.WriteLine("Got users");
            var user = userList.FirstOrDefault(x => (x.Username.ToLower() == userName.ToLower() || x.Id.ToString() == userName) && !x.IsBot);
            Console.WriteLine("Completed DM Query");
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
