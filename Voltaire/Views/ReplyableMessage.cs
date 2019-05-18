using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Views
{
    public static class ReplyableMessage
    {

        public static Tuple<string, Embed> Response(string username, string message, string reply)
        {

            var embed = new EmbedBuilder
            {
                Description = message + "\n\n `Reply With:`",
                Color = new Color(111, 111, 111),
                Footer = new EmbedFooterBuilder
                {
                    Text = $"send_reply+r {reply}"
                }
            };

            if (!string.IsNullOrEmpty(username))
            {
                embed.Author = new EmbedAuthorBuilder
                {
                    Name = username
                };
            }

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
