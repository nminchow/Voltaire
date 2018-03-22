using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Views.Info
{
    public static class Help
    {

        public static Tuple<string, Embed> Response(SocketCommandContext context)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Guide"
                },
                ThumbnailUrl = "https://nminchow.github.io/HeyListenWeb/images/demo/heylogo.png",
                Description = "Voltaire allows you to send messages to this discord guild (server) anonymously.\n\n" +
                "Most commands should be direct messaged to this bot user, which will then relay them to the desired channel.\n\n" +
                "**Direct Message Commands:**",
                Color = new Color(111, 111, 111)

            };

            embed.AddField("send {channel name} {message}", "Sends an anonymous message to the channel." +
                $"\nex: `send {ChannelName(context)} The cake is a lie.`");
            embed.AddField("send_guild {channel name} {message}", "This command is only needed if you belong to " +
                "multiple guilds that have Voltaire installed. It allows you to specify which guild you are sending to." +
                $"\nex: `send_guild \"{GuildName(context)}\" {ChannelName(context)} A man chooses, a slave obeys.`");
            embed.AddField("!volt help", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed);
        }

        private static string ChannelName(SocketCommandContext context)
        {
            return context.IsPrivate ? "some-channel" : context.Channel.Name;
        }

        private static string GuildName(SocketCommandContext context)
        {
            return context.IsPrivate ? "l33t g4merz" : context.Guild.Name;
        }
    }
}
