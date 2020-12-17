using Discord.Commands;
using System.Threading.Tasks;
using System;

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
            Console.WriteLine("got view");
            await Context.Channel.SendMessageAsync(view.Item1, embed: view.Item2);
        }

        [Command("link", RunMode = RunMode.Async)]
        [Summary("display invite link")]
        public async Task InviteLink()
        {
            await Context.Channel.SendMessageAsync("<https://discordapp.com/oauth2/authorize?client_id=425833927517798420&scope=bot>");
        }

        [Command("faq", RunMode = RunMode.Async)]
        [Summary("display faq link")]
        public async Task Faq()
        {
            await Context.Channel.SendMessageAsync("View the FAQ here: https://discordapp.com/channels/426894892262752256/581280324340940820/612849796025155585");
        }
    }
}
