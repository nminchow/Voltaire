using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Modules
{
    public class Info : ModuleBase<ShardedCommandContext>
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

        [Command("link", RunMode = RunMode.Async)]
        [Summary("display invite link")]
        public async Task InviteLink()
        {
            await Context.Channel.SendMessageAsync("<https://discordapp.com/oauth2/authorize?client_id=425833927517798420&scope=bot>");
        }
    }
}
