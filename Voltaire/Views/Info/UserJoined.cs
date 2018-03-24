using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Voltaire.Views.Info
{
    public static class UserJoined
    {

        public static Tuple<string, Embed> Response()
        {
            return new Tuple<string, Embed>("Looks like you just joined a server using Voltaire. Voltaire allows you to send messages to the server anonymously. Try typing: `help` to get rolling.", null);
        }
    }
}
