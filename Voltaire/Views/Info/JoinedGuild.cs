using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Views.Info
{
    public static class JoinedGuild
    {

        public static Tuple<string, Embed> Response()
        {
            return new Tuple<string, Embed>("Thanks for adding Voltaire to your server! Try: `!volt help` to get rolling.", null);
        }
    }
}
