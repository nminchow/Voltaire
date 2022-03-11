using System;
using System.Threading.Tasks;
using Discord.Interactions;
using Discord;

namespace Voltaire.Modules
{
    public class InfoInteractions : InteractionsBase
    {

        public InfoInteractions(DataBase database): base(database) {}


        [SlashCommand("volt-help", "get an overview of Voltaire's commands and functionality")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task VoltHelp(
            [Summary("private", "show the help dialogue privately")] Boolean? ephemeral = null
        )
        {
            var view = Views.Info.SlashHelp.Response(new InteractionBasedContext(Context, PublicResponder));
            await RespondAsync(view.Item1, embed: view.Item2, ephemeral: ephemeral == true);
        }

        [SlashCommand("volt-faq", "display the bot's faq link")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Faq()
        {
            await RespondAsync("View the FAQ here: https://discordapp.com/channels/426894892262752256/581280324340940820/612849796025155585");
        }

        [SlashCommand("volt-link", "display the bot's invite link")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Link()
        {
            await RespondAsync("<https://discordapp.com/oauth2/authorize?client_id=425833927517798420&scope=bot>");
        }
    }
}
