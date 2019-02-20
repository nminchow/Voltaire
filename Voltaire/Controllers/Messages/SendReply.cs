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
        public static async Task PerformAsync(SocketCommandContext context, string replyKey, string message, DataBase db)
        {
            var candidateGuilds = Send.GuildList(context);

            var key = LoadConfig.Instance.config["encryptionKey"];
            var candidateId = Rijndael.Decrypt(replyKey, key, KeySize.Aes256);

            var user = SendDirectMessage.ToUserList(candidateGuilds).Where(x => x.Id.ToString() == candidateId).FirstOrDefault();
            if(user == null)
            {
                await context.Channel.SendMessageAsync("Something is wrong with that reply code. It is possible the sender has left your server.");
                return;
            }

            var userGuild = FindOrCreateGuild.Perform(user.Guild, db);
            var prefix = PrefixHelper.ComputePrefix(context, userGuild, "someone");

            var channel = await user.GetOrCreateDMChannelAsync();
            await channel.SendMessageAsync($"{prefix} replied: {message}");
            await Send.SendSentEmote(context);
        }
    }
}
