using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voltaire.Views.Info
{
    public static class Help
    {

        public static Tuple<string, Embed> Response(ShardedCommandContext context)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Guide"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = "Voltaire allows you to send messages to a discord server anonymously.\n\n" +
                "Most commands should be direct messaged to this bot user, which will then relay them to the desired channel.\n\n" +
                "Support Server: https://discord.gg/xyzMyJH \n\n" +
                "**Direct Message Commands:**",
                Color = new Color(111, 111, 111),
                Footer = new EmbedFooterBuilder
                {
                    Text = "Developer note: the {user name}, {server name} and {channel name} arguments above can also be User, Server, and Channel IDs.",
                    IconUrl = ""
                }
            };

            embed.AddField("send {channel name} {message}", "Sends an anonymous message to the specified channel." +
                $"\nex: `send {ChannelName(context)} The cake is a lie.`");
            embed.AddField("send_dm {user name} {message}", "Sends an anonymous message to the specified user." +
                $"\nex: `send_dm @Voltaire The right man in the wrong place can make all the difference in the world.`");
            embed.AddField("send_server \"{server name}\" {channel name} {message}", "This command is only needed if you belong to " +
                "multiple servers that have Voltaire installed. It allows you to specify which server you are sending to." +
                $"\nex: `send_server \"{GuildName(context)}\" {ChannelName(context)} A man chooses, a slave obeys.`");
            embed.AddField("+r", "All 3 of the above 'send' commands also have a version which will allow other users to reply anonymously. " +
                "The reply version of the command is appended with a `+r` suffix." +
                $"\nex: `send_server+r \"{GuildName(context)}\" {ChannelName(context)} If we can just get back to Earth, and find Halsey, she can fix this.`");
            embed.AddField("react {message ID} {emote/emoji}",
                "Send a reaction to a message. [Enable dev settings to get message IDs](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID). " +
                "The `emote/emoji` param can be either a unicode emoji, or the name of a custom emote." +
                $"\nex: `react {context.Message.Id} 📜` or `react {context.Message.Id} your_custom_emote`");
            //embed.AddField("send_reply {reply code} {message}", "To reply to a message, it will need to have been originally sent with the +r suffix. The message will include" +
            //    "a code at the bottom which can be used to reply." +
            //    $"\nex: `send_reply iMIb62udZ7R/KCfhn634+AHvrrQ Don't make a girl a promise you know you can't keep.`");
            embed.AddField("!volt link", "Display the [bot's invite link](https://discordapp.com/oauth2/authorize?client_id=425833927517798420&permissions=2147998784&scope=bot%20applications.commands).");
            embed.AddField("!volt faq", "Display the [FAQ link](https://discordapp.com/channels/426894892262752256/581280324340940820/612849796025155585).");
            embed.AddField("!volt admin", "(server admin only - callable from server channel) Get a list of admin commands, including details on Voltaire Pro.");
            embed.AddField("!volt help", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed.Build());
        }

        private static string ChannelName(ShardedCommandContext context)
        {
            return context == null || context.IsPrivate ? "some-channel" : context.Channel.Name;
        }

        private static string GuildName(ShardedCommandContext context)
        {
            return  context.IsPrivate ? "l33t g4merz" : context.Guild.Name;
        }
    }
}
