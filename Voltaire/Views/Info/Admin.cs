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

        public static Tuple<string, Embed> Response(ShardedCommandContext context)
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

            embed.AddField("!volt allow_dm {true|false}", "Allow or disallow users to send direct messages to other members of the server." +
                $"\nex: `!volt allow_dm false`");

            embed.AddField("!volt permitted_role \"{role name}\"", "Only allow users with the supplied role to send server messages and direct messages on the server." +
                "Note that all users can still send replies. To clear, set the permitted role to @everyone." +
                $"\nex: `!volt permitted_role \"speakers of truth\"`");

            embed.AddField("!volt user_identifiers {true|false}", "Enable or disable the use of a unique (yet annonymous) identifier for users when they send messages." +
                $"\nex: `!volt user_identifiers false`");

            embed.AddField("!volt new_identifiers", "Generate new random identifiers for users. (Note: Banned users will still be banned.)" +
                $"\nex: `!volt new_identifiers`");

            embed.AddField("!volt pro", "Upgrade and monitor your Pro subscription." +
                $"\nex: `!volt pro`");

            embed.AddField("!volt ban {user id}", "Blacklists a user from the bot by user ID. This is the 4 digit number after their identifier when the \"user_identifiers\" setting is enabled." +
                $"\nex: `!volt ban 4321`");

            embed.AddField("!volt list_bans", "List currently blacklisted user IDs." +
                $"\nex: `!volt list_bans`");

            embed.AddField("!volt admin_role \"{role name}\"", "Set a role which can use admin commands and ban users on this server. (True server admins can always use commands as well)" +
                $"\nex: `!volt admin_role \"mods of truth\"`");

            embed.AddField("!volt admin", "(callable from anywhere) Display this help dialogue.");

            return new Tuple<string, Embed>("", embed.Build());
        }
    }
}
