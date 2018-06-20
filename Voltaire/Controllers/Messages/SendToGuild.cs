using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class SendToGuild
    {
        public static async Task LookupAndSendAsync(SocketGuild guild, ISocketMessageChannel currentChannel, string channelName, string message)
        {
            var chanel = guild.TextChannels.Where(x => x.Name.ToLower().Contains(channelName.ToLower()) || x.Id.ToString() == channelName);
            if (!chanel.Any())
            {
                await currentChannel.SendMessageAsync("The channel you specified couldn't be found. Please specify your channel using the following command: `send (channel_name) (message)` ex: `send some-channel you guys suck`");
            }
            await SendToChannel(currentChannel, chanel.OrderByDescending(x => x.Name.Length).First(), message);
        }

        public static async Task SendToChannel(ISocketMessageChannel currentChannel, SocketTextChannel channel, string message)
        {
            await channel.SendMessageAsync(message);
            await currentChannel.SendMessageAsync("Sent!");
        }

        public static async Task PerformAsync(SocketCommandContext context, string guildName, string channelName, string message)
        {
            var candidateGuilds = context.Client.Guilds.Where(x => x.Users.ToLookup(u => u.Id)[context.User.Id] != null && x.Name.ToLower().Contains(guildName.ToLower()));

            switch (candidateGuilds.Count())
            {
                case 0:
                    await context.Channel.SendMessageAsync("No guilds with the specified name could be found. The guils must have Voltaire installed and you must be a member of the guild.");
                    break;
                case 1:
                    await LookupAndSendAsync(candidateGuilds.First(), context.Channel, channelName, message);
                    break;
                default:
                    // check for exact match
                    var exactNameMatch = candidateGuilds.First(x => x.Name.ToLower() == guildName.ToLower());
                    if(exactNameMatch != null)
                    {
                        await LookupAndSendAsync(exactNameMatch, context.Channel, channelName, message);
                        return;
                    }
                    await context.Channel.SendMessageAsync("More than one guild with the spcified name was found. Please use a more specific guild name.");
                    break;
            }
        }
    }
}
