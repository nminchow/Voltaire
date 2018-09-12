using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voltaire.Controllers.Messages
{
    class SendDirectMessage
    {
        public static async Task PerformAsync(SocketCommandContext currentContext, string userName, string message, bool replyable ,DataBase db)
        {
            userName = userName.StartsWith('@') ? userName.Substring(1) : userName;
            try
            {
                var guildList = Send.GuildList(currentContext);
                List<SocketGuildUser> allUsersList = ToUserList(guildList);

                var userList = allUsersList.Where(x => x.Username != null && (x.Username.ToLower() == userName.ToLower() || x.Id.ToString() == userName) && !x.IsBot);

                var user = userList.Where(x => FilterGuild(x, db)).FirstOrDefault();

                if (user == null && userList.Any())
                {
                    await currentContext.Channel.SendMessageAsync("user found, but channel permissions do not allow annonymous direct messaging");
                    return;
                }
                else if (user == null)
                {
                    await currentContext.Channel.SendMessageAsync("user not found");
                    return;
                }

                var userChannel = await user.GetOrCreateDMChannelAsync();
                var prefix = PrefixHelper.ComputePrefix(currentContext, user.Guild, db, "anonymous user");
                var messageFunction = Send.SendMessageToChannel(userChannel, replyable, currentContext.User);
                await messageFunction(prefix, message);
                await currentContext.Channel.SendMessageAsync("Sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public static List<SocketGuildUser> ToUserList(IEnumerable<SocketGuild> guildList)
        {
            return guildList.Aggregate(new List<SocketGuildUser>(), (acc, item) => acc.Concat(item.Users).ToList());
        }

        private static bool FilterGuild(SocketGuildUser user, DataBase db)
        {
            return !db.Guilds.Any(x => x.DiscordId == user.Guild.Id.ToString() && !x.AllowDirectMessage);
        }
    }
}
