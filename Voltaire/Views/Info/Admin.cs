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
                    Name = "Admin Guide"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = "These commands are only callable by admin users.\n\n" +
                "Commands should be sent to the bot in a guild channel.\n\n" +
                "**Guild Channel Commands:**",
                Color = new Color(111, 111, 111)
            };

            embed.AddField("!volt allow_dm {true|false}", "Allow or Disallow users to send direct messages to other members of the guild." +
                $"\nex: `!volt allow_dm false`");
           
            embed.AddField("!volt admin", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed);
        }
    }
}
