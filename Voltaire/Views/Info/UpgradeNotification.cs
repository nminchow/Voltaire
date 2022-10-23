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
                Description = "Due to changes in the discord api, commands sent directly to Voltiare will soon quit functioning. " +
                "But fear not! New versions of all commands have been implemented via slash " +
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

    public static class UpgradeNotificationWithSendInfo
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
                Description = "Due to changes in the discord api, commands sent directly to Voltiare will soon quit functioning. \n\n" +
                "To send an anonymous message to a channel, the `/volt` command should be used in the server where that channel resides. " +
                "The command accepts a `channel` parameter, which will allow you to select a channel, or a `channel-name` parameter which will allow you to " +
                "specify a channel by name or id.\n\n" +
                "To get more info about slash commands, use the `/volt-help` command in a DM with the bot or in your target server.\n\n" +
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
