using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Preconditions
{
    class HasAllowedRole : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var db = (DataBase)services.GetService(typeof(DataBase));
            var guild = db.Guilds.FirstOrDefault(x => x.DiscordId == context.Guild.Id.ToString());
            if (guild == null || guild.AllowedRole == null)
            {
                return PreconditionResult.FromSuccess();
            }
            var role = context.Guild.Roles.FirstOrDefault(x => x.Id.ToString() == guild.AllowedRole);
            if (role == null)
            {
                return PreconditionResult.FromError("Voltaire is currently configured to only allow a specified role to message the server, and ");
            }
            return PreconditionResult.FromSuccess();
        }
    }
}
