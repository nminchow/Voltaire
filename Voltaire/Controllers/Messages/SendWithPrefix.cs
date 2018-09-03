using CodenameGenerator;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Rijndael256;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class SendWithPrefix
    {
        public static async Task Send(SocketCommandContext context, IMessageChannel channel, string message, DataBase db, string defaultPrefix = "")
        {
            if (UseUserIdentifier(context.Guild, db))
            {
                var generator = new Generator();

                string password = LoadConfig.Instance.config["encryptionKey"];
                var name = Rijndael.Encrypt((context.User.Id).ToString(), password, KeySize.Aes256);
                defaultPrefix = $"{name} says: ";
            }

            await channel.SendMessageAsync(defaultPrefix + message);
            await context.Channel.SendMessageAsync("Sent!");
        }

        private static bool UseUserIdentifier(SocketGuild guild, DataBase db)
        {
            return db.Guilds.FirstOrDefault(x => x.DiscordId == guild.Id.ToString())?.UseUserIdentifiers ?? false;
        }
    }
}
