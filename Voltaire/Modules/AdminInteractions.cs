using System;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Modules
{
    public class AdminInteractions
    {
        [Group("volt_admin", "Voltaire admin commands")]
        [Preconditions.Administrator]
        public class AdminGroup : InteractionsBase
        {
            public AdminGroup(DataBase database): base(database) {}

            // todo: add rest of the "administrator" settings here
            [SlashCommand("settings", "Configure Voltair's settings")]
            public async Task Settings(
                [Summary("allow-DM", "allow users to anonymously message one another via the bot")] Boolean? allowDM = null,
                [Summary("use-identifiers", "use a unique (yet anonymous) identifier for users when sending messages")] Boolean? identifiers = null
            )
            {
                Func<string, Discord.Embed, Task> SilentResponder = (response, embed) => { return Task.CompletedTask; };
                var context = new InteractionBasedContext(Context, SilentResponder);
                if (allowDM is Boolean allowDMvalue) {
                    Console.WriteLine("updated DM");
                    await Controllers.Settings.SetDirectMessageAccess.PerformAsync(context, allowDMvalue, _database);
                }
                if (identifiers is Boolean identifiersValue) {
                    Console.WriteLine("updated identifiers");
                    await Controllers.Settings.SetUseUserIdentifiers.PerformAsync(context, identifiersValue, _database);
                }
                await RespondAsync("settings updated!", ephemeral: true);
                return;
            }
        }


    }
}