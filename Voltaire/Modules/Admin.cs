using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

namespace Voltaire.Modules
{
    public class Admin : ModuleBase<ShardedCommandContext>
    {
        private DataBase _database;

        public Admin(DataBase database)
        {
            _database = database;
        }

        [Command("allow_dm", RunMode = RunMode.Async)]
        [Summary("Allow Direct Messages To Be Sent Annonymously Through This Server")]
        [Preconditions.Administrator]
        public async Task AllowDm(Boolean allow)
        {
            await Controllers.Settings.SetDirectMessageAccess.PerformAsync(new CommandBasedContext(Context), allow, _database);
        }

        [Command("user_identifiers", RunMode = RunMode.Async)]
        [Summary("Use a Unique (Yet Annonymous) Identifier For Users When Sending Messages")]
        [Preconditions.Administrator]
        public async Task UserIdentifiers(Boolean allow)
        {
            await Controllers.Settings.SetUseUserIdentifiers.PerformAsync(new CommandBasedContext(Context), allow, _database);
        }

        [Command("embeds", RunMode = RunMode.Async)]
        [Summary("Make All Messages Sent Via the Bot Appear as Embeds")]
        [Preconditions.Administrator]
        public async Task Embeds(Boolean allow)
        {
            await Controllers.Settings.SetEmbeds.PerformAsync(new CommandBasedContext(Context), allow, _database);
        }

        [Command("permitted_role", RunMode = RunMode.Async)]
        [Summary("Set the Role Allowed to Use Voltaire")]
        [Preconditions.Administrator]
        public async Task PermittedRole(SocketRole role)
        {
            await Controllers.Settings.SetAllowedRole.PerformAsync(new CommandBasedContext(Context), role, _database);
        }

        [Command("permitted_role all", RunMode = RunMode.Async)]
        [Summary("Allow All Users to Use Voltaire")]
        [Preconditions.Administrator]
        public async Task PermittedRoleClear()
        {
            await Controllers.Settings.ClearAllowedRole.PerformAsync(new CommandBasedContext(Context), _database);
        }

        [Command("new_identifiers", RunMode = RunMode.Async)]
        [Summary("Rotate User Identifiers")]
        [Preconditions.Administrator]
        public async Task NewIdentifiers()
        {
            await Controllers.Settings.GenerateGuildUserIdentifierSeed.PerformAsync(new CommandBasedContext(Context), _database);
        }

        [Command("ban", RunMode = RunMode.Async)]
        [Summary("Ban a given identifer seed")]
        [Preconditions.Administrator]
        public async Task Ban(string identifier)
        {
            await Controllers.Settings.BanIdentifier.PerformAsync(new CommandBasedContext(Context), identifier, _database);
        }

        [Command("unban", RunMode = RunMode.Async)]
        [Summary("Unban a given identifer seed")]
        [Preconditions.Administrator]
        public async Task UnBan(string identifier)
        {
            await Controllers.Settings.UnBanIdentifier.PerformAsync(new CommandBasedContext(Context), identifier, _database);
        }

        [Command("list_bans", RunMode = RunMode.Async)]
        [Summary("list current bans")]
        [Preconditions.Administrator]
        public async Task ListBans()
        {
            await Controllers.Settings.ListBans.PerformAsync(new CommandBasedContext(Context), _database);
        }

        [Command("clear_bans", RunMode = RunMode.Async)]
        [Summary("list current bans")]
        [Preconditions.Administrator]
        public async Task ClearBans()
        {
            await Controllers.Settings.ClearBans.PerformAsync(new CommandBasedContext(Context), _database);
        }

        // Require true admin
        [Command("admin_role", RunMode = RunMode.Async)]
        [Summary("Set the Role Allowed to Configure Voltaire and Ban Users")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AdminRole(SocketRole role)
        {
            await Controllers.Settings.SetAdminRole.PerformAsync(new CommandBasedContext(Context), role, _database);
        }

        // no precondition
        [Command("refresh", RunMode = RunMode.Async)]
        [Summary("Refresh the bot's user cache for this server")]
        public async Task Refresh()
        {
            await Controllers.Settings.Refresh.PerformAsync(new CommandBasedContext(Context), _database);
        }
    }
}
