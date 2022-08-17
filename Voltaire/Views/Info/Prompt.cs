using Discord;
using System;
using Voltaire.Models;

namespace Voltaire.Views.Info
{
    public static class Prompt
    {

        public static Tuple<string, Embed> Response(string prompt, Discord.WebSocket.SocketChannel channel, Discord.Interactions.ShardedInteractionContext context)
        {
            var text = prompt ?? $"Use the button below to send an anonymous message to \n <#{channel.Id}>";

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Voltaire Anonymous Messaging"
                },
                Description = text,
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Color = new Color(111, 111, 111),
            };
            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
