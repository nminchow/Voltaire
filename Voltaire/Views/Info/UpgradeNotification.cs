using Discord;
using System;
using Voltaire.Models;

namespace Voltaire.Views.Info
{
    public static class UpgradeNotification
    {

        public static Tuple<string, Embed> Response()
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Legacy Command Detected"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = "Unfortunately, the `!volt` command you just used will soon " +
                "quit functioning due to changes in the Discord API. But fear not! New versions of all commands have been implemented via slash " +
                "commands. To get more info about slash commands, use the `/volt-help` command in a DM with the bot or in your target server.\n\n" +
                "If slash commands aren't appearing in your server, one of your server admins will need to reinvite the bot with this link:\n"+
                "https://discordapp.com/oauth2/authorize?client_id=425833927517798420&permissions=2147998784&scope=bot%20applications.commands \n\n" +
                "If you are haveing any issues or would like more info, hop into our " +
                "Support Server: https://discord.gg/xyzMyJH",
                Color = new Color(111, 111, 111)
            };

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
