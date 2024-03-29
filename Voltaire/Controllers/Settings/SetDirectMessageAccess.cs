﻿using System;
using Voltaire.Controllers.Messages;
using System.Threading.Tasks;
using Voltaire.Controllers.Helpers;

namespace Voltaire.Controllers.Settings
{
    class SetDirectMessageAccess
    {
        public static async Task PerformAsync(UnifiedContext context, Boolean setting, DataBase db)
        {
            var guild = await FindOrCreateGuild.Perform(context.Guild, db);
            guild.AllowDirectMessage = setting;
            await db.SaveChangesAsync();
            await Send.SendMessageToContext(context, $"'Allow DM' set to {setting}");
        }
    }
}
