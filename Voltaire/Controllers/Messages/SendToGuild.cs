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
        public static async Task PerformAsync(ShardedCommandContext context, string guildName, string channelName, string message, bool replyable, DataBase db)
        {
            var unfilteredList = Send.GuildList(context);
            var candidateGuilds = unfilteredList.Where(x => x.Id.ToString() == guildName || x.Name.ToLower().Contains(guildName.ToLower()));

            switch (candidateGuilds.Count())
            {
                case 0:
                    await Send.SendErrorWithDeleteReaction(context, "No servers with the specified name could be found. The servers must have Voltaire installed and you must be a member of the server.");
                    break;
                case 1:
                    await LookupAndSendAsync(candidateGuilds.First(), context, channelName, message, replyable, db);
                    break;
                default:
                    // check for exact match
                    var exactNameMatch = candidateGuilds.First(x => x.Id.ToString() == guildName || x.Name.ToLower() == guildName.ToLower());
                    if (exactNameMatch != null)
                    {
                        await LookupAndSendAsync(exactNameMatch, context, channelName, message, replyable, db);
                        return;
                    }
                    await Send.SendErrorWithDeleteReaction(context, "More than one server with the spcified name was found. Please use a more specific server name.");
                    break;
            }
        }

        public static async Task LookupAndSendAsync(SocketGuild guild, ShardedCommandContext context, string channelName, string message, bool replyable, DataBase db)
        {
            var dbGuild = FindOrCreateGuild.Perform(guild, db);
            if (!UserHasRole.Perform(guild, context.User, dbGuild))
            {
                await Send.SendErrorWithDeleteReaction(context, "You do not have the role required to send messages to this server.");
                return;
            }

            var candidateChannels = guild.TextChannels.Where(x => x.Name.ToLower().Contains(channelName.ToLower()) || x.Id.ToString() == channelName);
            if (!candidateChannels.Any())
            {
                await Send.SendErrorWithDeleteReaction(context, "The channel you specified couldn't be found. Please specify your channel using the following command: `send (channel_name) (message)` ex: `send some-channel you guys suck`");
                return;
            }

            if (PrefixHelper.UserBlocked(context.User.Id, dbGuild))
            {
                await context.Channel.SendMessageAsync("It appears that you have been banned from using Voltaire on the targeted server. If you think this is an error, contact one of your admins.");
                return;
            }

            if(!IncrementAndCheckMessageLimit.Perform(dbGuild, db))
            {
                await Send.SendErrorWithDeleteReaction(context, "This server has reached its limit of 50 messages for the month. To lift this limit, ask an admin or moderator to upgrade your server to Voltaire Pro. (This can be done via the `!volt admin` command.)");
                return;
            }

            var prefix = PrefixHelper.ComputePrefix(context, dbGuild);
            var channel = candidateChannels.OrderBy(x => x.Name.Length).First();
            var messageFunction = Send.SendMessageToChannel(channel, replyable, context);
            await messageFunction(prefix, message);
            await Send.SendSentEmote(context);
            return;
        }
    }
}
