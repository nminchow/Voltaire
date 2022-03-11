using Discord;
using System;

namespace Voltaire.Views.Info
{
    public static class SlashAdmin
    {

        public static Tuple<string, Embed> Response(UnifiedContext context)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Admin Guide"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = "These commands are only callable by admin users.\n\n" +
                "Commands should be sent to the bot in a server channel.\n\n" +
                "**Guild Channel Commands:**",
                Color = new Color(111, 111, 111)
            };

            embed.AddField("/volt-admin settings", "Configure Voltaire's general settings, including DMs, identifiers, the use of embeds, and permitted role.");

            embed.AddField("/volt-admin new-identifiers ", "rotate user identifiers");

            embed.AddField("/pro", "Upgrade and monitor your Pro subscription.");

            embed.AddField("/volt-admin ban", "Blacklists a user from the bot by user ID. This is the 4 digit number after their identifier when the \"use-identifiers\" setting is enabled.");

            embed.AddField("/volt-admin unban", "Unban a user from the bot by user ID.");

            embed.AddField("/volt-admin list-bans list_bans", "List currently banned user IDs.");

            embed.AddField("/volt-admin clear-bans", "Clear the currently banned user list.");

            embed.AddField("/volt-admin refresh", "Refresh the user list for this server. This command isn't regularly necessary, but may be helpful when the bot first joins your server.");

            embed.AddField("/volt-admin role", "Set a role which can use admin commands and ban users on this server. (True server admins can always use admin commands as well)");

            embed.AddField("/volt-admin help", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
