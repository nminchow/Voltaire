using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Modules
{
    public class Info : ModuleBase<SocketCommandContext>
    {
        [Command("help", RunMode = RunMode.Async)]
        [Summary("get command overview")]
        public async Task GetInfo()
        {
            var view = Views.Info.Help.Response(Context);
            await Context.Channel.SendMessageAsync(view.Item1, embed: view.Item2);
        }

        [Command("admin", RunMode = RunMode.Async)]
        [Summary("get admin command overview")]
        public async Task AdminInfo()
        {
            var view = Views.Info.Admin.Response(Context);
            await Context.Channel.SendMessageAsync(view.Item1, embed: view.Item2);
        }
    }
}
