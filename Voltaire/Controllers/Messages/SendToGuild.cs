using System.Linq;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Messages
{
    class SendToGuild
    {
        public static async Task PerformAsync(UnifiedContext context, string guildName, string channelName, string message, bool replyable, DataBase db)
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

        public static async Task SendToChannelById(ulong channelId, UnifiedContext context, string message, bool replyable, DataBase db)
        {
            var unfilteredList = ToChannelList(Send.GuildList(context));
            var target = unfilteredList.FirstOrDefault(x => x.Id == channelId);
            await LookupAndSendAsync(target.Guild, context, channelId.ToString(), message, replyable, db);
            return;
        }

        public static List<SocketGuildChannel> ToChannelList(IEnumerable<SocketGuild> guildList)
        {
            return guildList.Aggregate(new List<SocketGuildChannel>(), (acc, item) => acc.Concat(item.Channels).ToList());
        }

        public static async Task LookupAndSendAsync(SocketGuild guild, UnifiedContext context, string channelName, string message, bool replyable, DataBase db)
        {
            var dbGuild = await FindOrCreateGuild.Perform(guild, db);
            if (!UserHasRole.Perform(guild, context.User, dbGuild))
            {
                await Send.SendErrorWithDeleteReaction(context, "You do not have the role required to send messages to this server.");
                return;
            }

            var candidateChannels = guild.TextChannels.Where(x => x.Name.ToLower().Contains(channelName.ToLower()) || x.Id.ToString() == channelName);
            if (!candidateChannels.Any())
            {
                await Send.SendErrorWithDeleteReaction(context, "The channel you specified couldn't be found. Please specify your desired channel before your message: `send (channel_name) (message)` ex: `send some-channel Nothing is true, everything is permitted.`");
                return;
            }

            if (PrefixHelper.UserBlocked(context.User.Id, dbGuild))
            {
                await Send.SendErrorWithDeleteReaction(context, "It appears that you have been banned from using Voltaire on the targeted server. If you think this is an error, contact one of your admins.");
                return;
            }

            if(! await IncrementAndCheckMessageLimit.Perform(dbGuild, db))
            {
                await Send.SendErrorWithDeleteReaction(context, "This server has reached its limit of 50 messages for the month. To lift this limit, ask an admin or moderator to upgrade your server to Voltaire Pro. (This can be done via the `/pro` command.)");
                return;
            }

            var prefix = PrefixHelper.ComputePrefix(context, dbGuild);
            var channel = candidateChannels.OrderBy(x => x.Name.Length).First();
            var messageFunction = Send.SendMessageToChannel(channel, replyable, context, dbGuild.UseEmbed);
            await messageFunction(prefix, message);
            await Send.SendSentEmoteIfCommand(context);
            return;
        }
    }
}
