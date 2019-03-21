using Discord.Commands;
using Discord.WebSocket;
using DiscordBotsList.Api;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Voltaire.Controllers.Helpers
{
    public static class JoinedGuild
    {
        public static Func<SocketGuild, Task> Joined(DataBase db, string token)
        {
            Func<SocketGuild, Task> convert = async delegate (SocketGuild guild)
            {
                IConfiguration configuration = LoadConfig.Instance.config;

                FindOrCreateGuild.Perform(guild, db);

                var view = Views.Info.JoinedGuild.Response();
                await guild.TextChannels.First().SendMessageAsync(text: view.Item1, embed: view.Item2);

                AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(425833927517798420, token);
                var me = await DblApi.GetMeAsync();
                await me.UpdateStatsAsync(db.Guilds.Count());
            };

            return convert;
        }
    }
}
