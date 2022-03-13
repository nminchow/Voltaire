using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Voltaire.Controllers.Helpers
{
    public static class JoinedGuild
    {

        public static void Refresh(SocketGuild guild)
        {
            Task.Run(async () => {
                Console.WriteLine("downloading user list");
                await guild.DownloadUsersAsync();
                Console.WriteLine("user list downloaded");
            });
        }

        public static Func<SocketGuild, Task> Joined(DataBase db)
        {
            Func<SocketGuild, Task> convert = async delegate (SocketGuild guild)
            {
                IConfiguration configuration = LoadConfig.Instance.config;

                Refresh(guild);

                FindOrCreateGuild.Perform(guild, db);

                var view = Views.Info.JoinedGuild.Response();
                await guild.TextChannels.First().SendMessageAsync(text: view.Item1, embed: view.Item2);

                // AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(425833927517798420, token);
                // var me = await DblApi.GetMeAsync();
                // await me.UpdateStatsAsync(db.Guilds.Count());
            };

            return convert;
        }
    }
}
