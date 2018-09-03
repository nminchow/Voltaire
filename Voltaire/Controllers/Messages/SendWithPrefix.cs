using CodenameGenerator;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class SendWithPrefix
    {
        public static async Task Send(SocketCommandContext context, IMessageChannel channel, SocketGuild settingSource, string message, DataBase db, string defaultPrefix = "")
        {
            if (UseUserIdentifier(settingSource, db))
            {
                string password = LoadConfig.Instance.config["encryptionKey"];

                var cipher = new RijndaelManaged()
                {
                    Padding = PaddingMode.Zeros,
                    Mode = CipherMode.ECB,
                    KeySize = 256,
                    Key = Convert.FromBase64String(password)
                };

                var transform = cipher.CreateEncryptor();
                var id = (context.User.Id % 100000).ToString();
                var bytes = transform.TransformFinalBlock(Encoding.UTF8.GetBytes(id), 0, id.Length);

                // get the first bytes so adjacent IDs are still different, just in case
                var integer = BitConverter.ToInt32(bytes, bytes.Length - 4);
                var generator = new Generator(seed: integer)
                {
                    Casing = Casing.PascalCase,
                    Parts = new WordBank[] { WordBank.Adverbs, WordBank.Verbs, WordBank.Nouns }
                };
                defaultPrefix = $"{generator.Generate()} says: ";
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
