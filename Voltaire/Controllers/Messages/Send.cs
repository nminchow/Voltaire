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
        public static async Task PerformAsync(ShardedCommandContext context, string channelName, string message, bool reply, DataBase db)
        {
            var candidateGuilds = GuildList(context);
            switch (candidateGuilds.Count())
            {
                case 0:
                    await SendErrorWithDeleteReaction(context, "It doesn't look like you belong to any servers where Voltaire is installed. Please add Voltaire to your desired server.");
                    break;
                case 1:
                    await SendToGuild.LookupAndSendAsync(candidateGuilds.First(), context, channelName, message, reply, db);
                    break;
                default:
                    await SendErrorWithDeleteReaction(context, "It looks like you belong to multiple servers where Voltaire is installed. Please specify your server using the following command: `send_server (server_name) (channel_name) (message)` ex: `send_server \"l33t g4amerz\" some-channel you guys suck`");
                    break;
            }
        }

        public static Func<string, string, Task> SendMessageToChannel(IMessageChannel channel, bool replyable, ShardedCommandContext context)
        {
            if (!replyable)
            {
                return async (username, message) =>
                {
                    message = CheckForMentions(channel, message);
                    if (string.IsNullOrEmpty(username))
                    {
                        await SendMessageAndCatchError(() => { return channel.SendMessageAsync(message); }, context);
                        return;
                    }
                    await SendMessageAndCatchError(() => { return channel.SendMessageAsync($"**{username}**: {message}"); }, context);
                };
            }
            return async (username, message) =>
            {
                var key = LoadConfig.Instance.config["encryptionKey"];
                var replyHash = Rijndael.Encrypt(context.User.Id.ToString(), key, KeySize.Aes256);
                var view = Views.ReplyableMessage.Response(username, message, replyHash.ToString());
                await SendMessageAndCatchError(() => { return channel.SendMessageAsync(view.Item1, embed: view.Item2); }, context);
            };
        }

        public static async Task SendMessageAndCatchError(Func<Task> send, ShardedCommandContext context)
        {
            try
            {
                await send();
            }
            catch (Discord.Net.HttpException e)
            {
                switch (e.DiscordCode)
                {
                    case 50007:
                        await context.Channel.SendMessageAsync("Voltaire has been blocked by this user.");
                        break;
                    case 50013:
                    case 50001:
                        await context.Channel.SendMessageAsync("Voltaire doesn't have the " +
                        "permissions required to send this message. Ensure Voltaire can access the channel you are tyring to send to, and that it has " +
                        " \"Embed Links\" and \"Use External Emojis\" permission.");
                        break;
                }

                throw e;
            }
        }

        private static string CheckForMentions(IMessageChannel channel, string message)
        {
            var words = message.Split().Where(x => x.StartsWith("@"));
            if (!words.Any())
                return message;

            var users = AsyncEnumerableExtensions.Flatten(channel.GetUsersAsync());

            users.Select(x => $"@{x.Username}").Intersect(words.ToAsyncEnumerable()).ForEach(async x =>
            {
                var user = await users.First(y => y.Username == x.Substring(1));
                message = message.Replace(x, user.Mention);
            });

            return message;
        }

        public static IEnumerable<SocketGuild> GuildList(ShardedCommandContext currentContext)
        {
            return currentContext.Client.Guilds.Where(x => x.Users.Any(u => u.Id == currentContext.User.Id));
        }

        public static async Task SendSentEmote(ShardedCommandContext context)
        {
            var emote = Emote.Parse(LoadConfig.Instance.config["sent_emoji"]);
            await context.Message.AddReactionAsync(emote);
        }

        public static async Task SendErrorWithDeleteReaction(ShardedCommandContext context, string errorMessage)
        {
            var message = await context.Channel.SendMessageAsync(errorMessage);
            var emote = new Emoji(DeleteEmote);
            await message.AddReactionAsync(emote);
        }

        public static string DeleteEmote = "🗑";
    }
}
