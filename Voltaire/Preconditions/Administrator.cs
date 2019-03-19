using System;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Voltaire.Controllers.Helpers;
using Discord;

namespace Voltaire.Preconditions
{
    class Administrator : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // possible to pass the context in?
            var db = services.GetService<DataBase>();
            var guild = FindOrCreateGuild.Perform(context.Guild, db);

            if (guild.AdminRole != null)
            {
                var role = context.Guild.Roles.FirstOrDefault(x => x.Id.ToString() == guild.AdminRole);
                if (role != null)
                {
                    var p = (SocketRole)role;
                    if(p.Members.Any(x => x.Id == context.User.Id))
                    {
                        return PreconditionResult.FromSuccess();
                    }
                }
            }

            return await new RequireUserPermissionAttribute(GuildPermission.Administrator).CheckPermissionsAsync(context, command, services);
        }
    }
}
