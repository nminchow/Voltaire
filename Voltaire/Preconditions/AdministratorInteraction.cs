using System;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;
using Voltaire.Controllers.Helpers;
using Discord;
using Discord.Interactions;

namespace Voltaire.Preconditions
{      class AdministratorInteraction : Discord.Interactions.PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, Discord.Interactions.ICommandInfo commandInfo, IServiceProvider services)
        {
            Console.WriteLine("in precondition check!");
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
            Console.WriteLine("returning default");
            return await new RequireUserPermissionAttribute(GuildPermission.Administrator).CheckRequirementsAsync(context, commandInfo, services);
        }
    }
}