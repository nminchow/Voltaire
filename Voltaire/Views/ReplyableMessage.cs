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

        public static Tuple<string, Embed> Response(string message, string reply)
        {

            var embed = new EmbedBuilder
            {
                Description = message,
                Color = new Color(111, 111, 111),
                Footer = new EmbedFooterBuilder
                {
                    Text = $"send_reply {reply}"
                }
            };

            return new Tuple<string, Embed>("", embed);
        }
    }
}
