using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voltaire.Models;

namespace Voltaire.Controllers.Helpers
{
    class FindOrCreateGuild
    {
        public static Guild Perform(IGuild guild, DataBase db)
        {
            var dbGuild = db.Guilds.Include(x => x.BannedIdentifiers).FirstOrDefault(u => u.DiscordId == guild.Id.ToString());
            if (dbGuild == null)
            {
                dbGuild = new Guild { DiscordId = guild.Id.ToString() };
                db.Guilds.Add(dbGuild);
                db.SaveChanges();
            }
            return dbGuild;
        }
    }
}
