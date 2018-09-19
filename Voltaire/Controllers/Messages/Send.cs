using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Rijndael256;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class Send
    {
        public static async Task PerformAsync(SocketCommandContext context, string channelName, string message, bool reply, DataBase db)
        {
            var candidateGuilds = GuildList(context);
            switch (candidateGuilds.Count())
            {
                case 0:
                    await context.Channel.SendMessageAsync("It doesn't look like you belong to any guilds(servers) where Voltaire is installed. Please add Voltaire to your desired server.");
                    break;
                case 1:
                    await SendToGuild.LookupAndSendAsync(candidateGuilds.First(), context, channelName, message, reply, db);
                    break;
                default:
                    await context.Channel.SendMessageAsync("It looks like you belong to multiple guilds(servers) where Voltaire is installed. Please specify your guild using the following command: `send_guild (guild_name) (channel_name) (message)` ex: `send_guild \"l33t g4amerz\" some-channel you guys suck`");
                    break;
            }
        }

        public static Func<string, string, Task> SendMessageToChannel(IMessageChannel channel, bool reply, SocketUser user)
        {
            if (!reply)
            {
                return async (username, message) =>
                {
                    if (string.IsNullOrEmpty(username))
                    {
                        await channel.SendMessageAsync(message);
                        return;
                    }
                    await channel.SendMessageAsync($"**{username}**: {message}");
                };
            }
            return async (username, message) =>
            {
                var key = LoadConfig.Instance.config["encryptionKey"];
                var replyHash = Rijndael.Encrypt(user.Id.ToString(), key, KeySize.Aes256);
                var view = Views.ReplyableMessage.Response(username, message, replyHash.ToString());
                await channel.SendMessageAsync(view.Item1, embed: view.Item2);
            };
        }

        public static IEnumerable<SocketGuild> GuildList(SocketCommandContext currentContext)
        {
            return currentContext.Client.Guilds.Where(x => x.Users.ToLookup(u => u.Id).Contains(currentContext.User.Id));
        }

        public static async Task SendSentEmote(SocketCommandContext context)
        {
            var emote = Emote.Parse(LoadConfig.Instance.config["sent_emoji"]);
            await context.Message.AddReactionAsync(emote);
        }
    }
}
