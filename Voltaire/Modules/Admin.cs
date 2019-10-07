using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;
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
            await Controllers.Settings.SetDirectMessageAccess.PerformAsync(Context, allow, _database);
        }

        [Command("user_identifiers", RunMode = RunMode.Async)]
        [Summary("Use a Unique (Yet Annonymous) Identifier For Users When Sending Messages")]
        [Preconditions.Administrator]
        public async Task UserIdentifiers(Boolean allow)
        {
            await Controllers.Settings.SetUseUserIdentifiers.PerformAsync(Context, allow, _database);
        }

        [Command("permitted_role", RunMode = RunMode.Async)]
        [Summary("Set the Role Allowed to Use Voltaire")]
        [Preconditions.Administrator]
        public async Task PermittedRole(SocketRole role)
        {
            await Controllers.Settings.SetAllowedRole.PerformAsync(Context, role, _database);
        }

        [Command("permitted_role all", RunMode = RunMode.Async)]
        [Summary("Allow All Users to Use Voltaire")]
        [Preconditions.Administrator]
        public async Task PermittedRoleClear()
        {
            await Controllers.Settings.ClearAllowedRole.PerformAsync(Context, _database);
        }

        [Command("new_identifiers", RunMode = RunMode.Async)]
        [Summary("Rotate User Identifiers")]
        [Preconditions.Administrator]
        public async Task NewIdentifiers()
        {
            await Controllers.Settings.GenerateGuildUserIdentifierSeed.PerformAsync(Context, _database);
        }

        [Command("ban", RunMode = RunMode.Async)]
        [Summary("Ban a given identifer seed")]
        [Preconditions.Administrator]
        public async Task Ban(string identifier)
        {
            await Controllers.Settings.BanIdentifier.PerformAsync(Context, identifier, _database);
        }

        [Command("unban", RunMode = RunMode.Async)]
        [Summary("Unban a given identifer seed")]
        [Preconditions.Administrator]
        public async Task UnBan(string identifier)
        {
            await Controllers.Settings.UnBanIdentifier.PerformAsync(Context, identifier, _database);
        }

        [Command("list_bans", RunMode = RunMode.Async)]
        [Summary("list current bans")]
        [Preconditions.Administrator]
        public async Task ListBans()
        {
            await Controllers.Settings.ListBans.PerformAsync(Context, _database);
        }

        // Require true admin
        [Command("admin_role", RunMode = RunMode.Async)]
        [Summary("Set the Role Allowed to Configure Voltaire and Ban Users")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AdminRole(SocketRole role)
        {
            await Controllers.Settings.SetAdminRole.PerformAsync(Context, role, _database);
        }
    }
}
