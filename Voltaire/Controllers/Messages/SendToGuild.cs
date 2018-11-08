using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Messages
{
    class SendToGuild
    {
        public static async Task PerformAsync(SocketCommandContext context, string guildName, string channelName, string message, bool replyable, DataBase db)
        {
            var unfilteredList = Send.GuildList(context);
            var candidateGuilds = unfilteredList.Where(x => x.Name.ToLower().Contains(guildName.ToLower()));

            switch (candidateGuilds.Count())
            {
                case 0:
                    await context.Channel.SendMessageAsync("No servers with the specified name could be found. The servers must have Voltaire installed and you must be a member of the server.");
                    break;
                case 1:
                    await LookupAndSendAsync(candidateGuilds.First(), context, channelName, message, replyable, db);
                    break;
                default:
                    // check for exact match
                    var exactNameMatch = candidateGuilds.First(x => x.Name.ToLower() == guildName.ToLower());
                    if (exactNameMatch != null)
                    {
                        await LookupAndSendAsync(exactNameMatch, context, channelName, message, replyable, db);
                        return;
                    }
                    await context.Channel.SendMessageAsync("More than one server with the spcified name was found. Please use a more specific server name.");
                    break;
            }
        }

        public static async Task LookupAndSendAsync(SocketGuild guild, SocketCommandContext context, string channelName, string message, bool replyable, DataBase db)
        {
            var dbGuild = FindOrCreateGuild.Perform(guild, db);
            if (!UserHasRole.Perform(guild, context.User, dbGuild))
            {
                await context.Channel.SendMessageAsync("You do not have the role required to send messages to this server.");
                return;
            }

            var candidateChannels = guild.TextChannels.Where(x => x.Name.ToLower().Contains(channelName.ToLower()) || x.Id.ToString() == channelName);
            if (!candidateChannels.Any())
            {
                await context.Channel.SendMessageAsync("The channel you specified couldn't be found. Please specify your channel using the following command: `send (channel_name) (message)` ex: `send some-channel you guys suck`");
                return;
            }

            var prefix = PrefixHelper.ComputePrefix(context, dbGuild);
            var channel = candidateChannels.OrderBy(x => x.Name.Length).First();
            var messageFunction = Send.SendMessageToChannel(channel, replyable, context.User);
            await messageFunction(prefix, message);
            await Send.SendSentEmote(context);
        }
    }
}
