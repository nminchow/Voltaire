using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rijndael256;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Messages
{
    class SendReply
    {
        public static async Task PerformAsync(ShardedCommandContext context, string replyKey, string message, bool replyable, DataBase db)
        {
            var candidateGuilds = Send.GuildList(context);

            var key = LoadConfig.Instance.config["encryptionKey"];
            var candidateId = Rijndael.Decrypt(replyKey, key, KeySize.Aes256);

            // TODO: potentially want to bake guilds into reply codes so we can ensure that the the replier isn't banned on the server where the original
            // message was sent
            var users = SendDirectMessage.ToUserList(candidateGuilds).Where(x => x.Id.ToString() == candidateId);
            if(users.Count() == 0)
            {
                await Send.SendErrorWithDeleteReaction(context, "Something is wrong with that reply code. It is possible the sender has left your server.");
                return;
            }

            var allowedGuild = users.ToList().Select(x => FindOrCreateGuild.Perform(x.Guild, db)).FirstOrDefault(x => !PrefixHelper.UserBlocked(context.User.Id, x));

            if (allowedGuild == null)
            {
                await context.Channel.SendMessageAsync("It appears that you have been banned from using Voltaire on the targeted server. If you think this is an error, contact one of your admins.");
                return;
            }

            var prefix = $"{PrefixHelper.ComputePrefix(context, allowedGuild, "someone")} replied";

            // all 'users' here are technically the same user, so just take the first
            var channel = await users.First().GetOrCreateDMChannelAsync();
            var messageFunction = Send.SendMessageToChannel(channel, replyable, context);
            var sentMessage = await messageFunction(prefix, message);
            await Send.AddReactionToMessage(sentMessage);
            await Send.SendSentEmote(context);
        }
    }
}
