using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voltaire.Models;

namespace Voltaire.Controllers.Helpers
{
    class FindOrCreateGuild
    {
        public static Guild Perform(SocketGuild guild, DataBase db)
        {
            var dbGuild = db.Guilds.FirstOrDefault(u => u.DiscordId == guild.Id.ToString());
            if (dbGuild == null)
            {
                dbGuild = new Guild { DiscordId = guild.Id.ToString() };
                db.Guilds.Add(dbGuild);
            }
            return dbGuild;
        }
    }
}
