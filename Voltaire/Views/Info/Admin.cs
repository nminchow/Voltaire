using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Views.Info
{
    public static class Admin
    {

        public static Tuple<string, Embed> Response(SocketCommandContext context)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Guide"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Color = new Color(111, 111, 111),
                Footer = new EmbedFooterBuilder
                {
                    Text = "Note: these commands must be send to a guild channel (not via DM to the bot)",
                    IconUrl = ""
                }
            };

            embed.AddField("allow_dm {true|false}", "Allow/Disallow users to send direct messages to other members of the channel." +
                $"\nex: `allow_dm false`");
           
            embed.AddField("!volt admin", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed);
        }

        private static string ChannelName(SocketCommandContext context)
        {
            return context == null || context.IsPrivate ? "some-channel" : context.Channel.Name;
        }

        private static string GuildName(SocketCommandContext context)
        {
            return  context.IsPrivate ? "l33t g4merz" : context.Guild.Name;
        }
    }
}
