using Discord;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Voltaire.Models;
using System.Threading.Tasks;
using System;

namespace Voltaire.Controllers.Helpers
{
    class FindOrCreateGuild
    {
        public static async Task<Guild> Perform(IGuild guild, DataBase db)
        {
            var dbGuild = await db.Guilds.Include(x => x.BannedIdentifiers).FirstOrDefaultAsync(u => u.DiscordId == guild.Id.ToString());
            if (dbGuild == null)
            {
                dbGuild = new Guild { DiscordId = guild.Id.ToString() };
                await db.Guilds.AddAsync(dbGuild);
                await db.SaveChangesAsync();
            }
            return dbGuild;
        }
    }
}
