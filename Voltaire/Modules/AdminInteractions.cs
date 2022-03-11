using System;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Voltaire.Modules
{
    public class AdminInteractions
    {
        [Group("volt-admin", "Voltaire admin commands")]
        public class AdminGroup : InteractionsBase
        {
            public AdminGroup(DataBase database): base(database) {}

            [SlashCommand("settings", "configure Voltaire's general settings")]
            [Preconditions.AdministratorInteraction]
            public async Task Settings(
                [Summary("allow-DM", "allow users to anonymously message one another via the bot")] Boolean? allowDM = null,
                [Summary("use-identifiers", "use a unique (yet anonymous) identifier for users when sending messages")] Boolean? identifiers = null,
                [Summary("embeds", "make all messages sent via the bot appear as embeds")] Boolean? embeds = null,
                [Summary("permitted-role", "set the role allowed to use voltaire")] SocketRole role = null
            )
            {
                Func<string, Discord.Embed, Task> SilentResponder = (response, embed) => { return Task.CompletedTask; };
                var context = new InteractionBasedContext(Context, SilentResponder);
                if (allowDM is Boolean allowDMvalue) {
                    await Controllers.Settings.SetDirectMessageAccess.PerformAsync(context, allowDMvalue, _database);
                }
                if (identifiers is Boolean identifiersValue) {
                    await Controllers.Settings.SetUseUserIdentifiers.PerformAsync(context, identifiersValue, _database);
                }
                if (embeds is Boolean embedsValue) {
                    await Controllers.Settings.SetEmbeds.PerformAsync(context, embedsValue, _database);
                }
                if (role is SocketRole roleValue) {
                    Console.WriteLine("setting allowed role");
                    await Controllers.Settings.SetAllowedRole.PerformAsync(context, roleValue, _database);
                }
                await RespondAsync("settings updated!", ephemeral: true);
                return;
            }

            [SlashCommand("new-identifiers", "rotate user identifiers")]
            [Preconditions.AdministratorInteraction]
            public async Task RotateIdentifiers()
            {
                await Controllers.Settings.GenerateGuildUserIdentifierSeed.PerformAsync(new InteractionBasedContext(Context, Responder), _database);
            }

            [SlashCommand("ban", "ban a given identifier")]
            [Preconditions.AdministratorInteraction]
            public async Task Ban(string identifier)
            {
                await Controllers.Settings.BanIdentifier.PerformAsync(new InteractionBasedContext(Context, Responder), identifier, _database);
            }

            [SlashCommand("unban", "unban a given identifier")]
            [Preconditions.AdministratorInteraction]
            public async Task UnBan(string identifier)
            {
                await Controllers.Settings.UnBanIdentifier.PerformAsync(new InteractionBasedContext(Context, Responder), identifier, _database);
            }

            [SlashCommand("list-bans", "list current bans")]
            [Preconditions.AdministratorInteraction]
            public async Task ListBans()
            {
                await Controllers.Settings.ListBans.PerformAsync(new InteractionBasedContext(Context, Responder), _database);
            }

            [SlashCommand("clear-bans", "clear current bans")]
            [Preconditions.AdministratorInteraction]
            public async Task ClearBans()
            {
                await Controllers.Settings.ClearBans.PerformAsync(new InteractionBasedContext(Context, Responder), _database);
            }

            [SlashCommand("refresh", "refresh the bot's user cache for this server")]
            public async Task Refresh()
            {
                await Controllers.Settings.Refresh.PerformAsync(new InteractionBasedContext(Context, Responder), _database);
            }

            [SlashCommand("role", "set the Role Allowed to Configure Voltaire and Ban Users")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task AdminRole(SocketRole role)
            {
                await Controllers.Settings.SetAdminRole.PerformAsync(new InteractionBasedContext(Context, Responder), role, _database);
            }

            [SlashCommand("help", "get admin command overview")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task Help(
                [Summary("private", "show the help dialogue privately")] Boolean? ephemeral = null
            )
            {
                var view = Views.Info.SlashAdmin.Response(new InteractionBasedContext(Context, Responder));
                await RespondAsync(view.Item1, embed: view.Item2, ephemeral: ephemeral == true);
            }

        }


    }
}