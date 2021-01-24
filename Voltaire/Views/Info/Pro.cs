using Discord;
using System;
using Voltaire.Models;

namespace Voltaire.Views.Info
{
    public static class Pro
    {

        public static Tuple<string, Embed> Response(string url, Guild guild, DataBase db)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Upgrade To Voltaire Pro With the Folling URL:"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = $"{url}",
                Color = new Color(111, 111, 111)
            };

            Controllers.Helpers.IncrementAndCheckMessageLimit.CheckMonth(guild);

            embed.AddField("Messages Sent This Month:", $"**{guild.MessagesSentThisMonth}**/50 (note: any messages sent above the limit have been surpressed)");

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
