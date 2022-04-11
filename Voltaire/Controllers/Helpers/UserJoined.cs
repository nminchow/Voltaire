using Discord.WebSocket;
using System.Threading.Tasks;


namespace Voltaire.Controllers.Helpers
{
    public static class UserJoined
    {
        public static async Task SendJoinedMessage(SocketGuildUser user)
        {
            if (user.IsBot)
                return;
            var view = Views.Info.UserJoined.Response();
            var channel = await user.CreateDMChannelAsync();
            await channel.SendMessageAsync(text: view.Item1, embed: view.Item2);
        }
    }
}
