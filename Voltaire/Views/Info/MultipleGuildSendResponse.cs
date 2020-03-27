using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voltaire.Views.Info
{
    public static class MultipleGuildSendResponse
    {

        public static Tuple<string, Embed> Response(ShardedCommandContext context, IEnumerable<SocketGuild> guilds, string message)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "send_server"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = "It looks like you belong to multiple servers where Voltaire is installed. Please specify your server using the following command: `send_server (server_name) (channel_name) (message)`\n\n" +
                "**Servers:**",
                Color = new Color(111, 111, 111)
            };

            guilds.Select(x => {
                embed.AddField(x.Name, $"ex: `!volt send_server \"{x.Name}\" {x.Channels.FirstOrDefault().Name} {message}`");
                return true;
            }).ToList();

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
