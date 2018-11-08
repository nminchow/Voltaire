using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Voltaire.Models;

namespace Voltaire.Controllers.Helpers
{
    class UserHasRole
    {
        public static bool Perform(SocketGuild reciver, IUser sender, Guild receivingRecord)
        {
            if (receivingRecord == null || receivingRecord.AllowedRole == null)
            {
                return true;
            }
            var role = reciver.Roles.FirstOrDefault(x => x.Id.ToString() == receivingRecord.AllowedRole);
            if (role == null)
            {
                return false;
            }

            return role.Members.Any(x => x.Id == sender.Id);
        }
    }
}
