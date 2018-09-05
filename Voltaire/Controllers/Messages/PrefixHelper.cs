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
    class PrefixHelper
    {

        public static string ComputePrefix(SocketCommandContext context, SocketGuild guild, DataBase db, string defaultValue = "")
        {
            return UseUserIdentifier(guild, db) ? Generate(context) : defaultValue;
        }

        public static string Generate(SocketCommandContext context)
        {
            string password = LoadConfig.Instance.config["encryptionKey"];

            var offset = (ulong)(new Random().Next(10, 30));
            var id = (context.User.Id + offset).ToString();

            var bytes = GetHash(id, password);

            var resultString = BitConverter.ToString(bytes);

            var integer = BitConverter.ToInt32(bytes, bytes.Length - 4);

            //Console.WriteLine($"{resultString} {integer} {offset}");

            var generator = new Generator(seed: integer)
            {
                Casing = Casing.PascalCase,
                Parts = new WordBank[] { WordBank.Adverbs, WordBank.Verbs, WordBank.Nouns }
            };
            return $"{generator.Generate()} says: ";
        }

        public static Byte[] GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return hashBytes;
        }

        public static bool UseUserIdentifier(SocketGuild guild, DataBase db)
        {
            return db.Guilds.FirstOrDefault(x => x.DiscordId == guild.Id.ToString())?.UseUserIdentifiers ?? false;
        }
    }
}
