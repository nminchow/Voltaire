using Discord;
using Discord.Commands;
using System;

namespace Voltaire.Views.Info
{
    public static class SlashHelp
    {

        public static Tuple<string, Embed> Response(UnifiedContext context)
        {

            var embed = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    Name = "Guide"
                },
                ThumbnailUrl = "https://nminchow.github.io/VoltaireWeb/images/quill.png",
                Description = "Voltaire allows you to send messages to a discord server anonymously.\n\n" +
                "Most commands should be direct messaged to this bot user, which will then relay them to the desired channel.\n\n" +
                "Support Server: https://discord.gg/xyzMyJH \n\n" +
                "**Direct Message Commands:**",
                Color = new Color(111, 111, 111),
                Footer = new EmbedFooterBuilder
                {
                    Text = "Developer note: the {user name}, {server name} and {channel name} arguments above can also be User, Server, and Channel IDs.",
                    IconUrl = ""
                }
            };

            embed.AddField("/volt", "Sends an anonymous message to the current channel.");
            embed.AddField("/send-dm", "Sends an anonymous message to the specified user.");
            embed.AddField("/send", "Sends an anonymous message to the specified channel in the current server.");
            embed.AddField("/send-reply", "Sends a reply to the specified message in the current server.");

            embed.AddField("/react",
                "Send a reaction to a message. [Enable dev settings to get message IDs](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID). " +
                "The `emote/emoji` param can be either a unicode emoji, or the name of a custom emote.");

            embed.AddField("/volt-link", "Display the [bot's invite link](https://discordapp.com/oauth2/authorize?client_id=425833927517798420&scope=bot).");
            embed.AddField("/volt-faq", "Display the [FAQ link](https://discordapp.com/channels/426894892262752256/581280324340940820/612849796025155585).");
            embed.AddField("/volt-admin help", "Get a list of admin commands, including details on Voltaire Pro.");
            embed.AddField("/volt-help", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
