using Discord.Commands;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Modules
{
    public class Messages : ModuleBase<ShardedCommandContext>
    {

        private DataBase _database;

        public Messages(DataBase database)
        {
            _database = database;
        }

        [Command("send", RunMode = RunMode.Async)]
        [Summary("error message for malformed send")]
        public async Task SendError(string _one)
        {
            await Controllers.Messages.Send.SendErrorWithDeleteReaction(new CommandBasedContext(Context), "Please specify your channel name, ex: `send some-channel hello`");
        }

        [Command("send", RunMode = RunMode.Async)]
        public async Task Send(string channelName, [Remainder] string message)
        {
            await Controllers.Messages.Send.PerformAsync(new CommandBasedContext(Context), channelName, message, false, _database);
        }

        [Command("send+r", RunMode = RunMode.Async)]
        public async Task SendRepliable(string channelName, [Remainder] string message)
        {
            await Controllers.Messages.Send.PerformAsync(new CommandBasedContext(Context), channelName, message, true, _database);
        }

        [Command("send_server", RunMode = RunMode.Async)]
        public async Task SendServer(string guildName, string channelName, [Remainder] string message)
        {
            await SendToGuild.PerformAsync(new CommandBasedContext(Context), guildName, channelName, message, false, _database);
        }

        [Command("send_server+r", RunMode = RunMode.Async)]
        public async Task SendServerRepliable(string guildName, string channelName, [Remainder] string message)
        {
            await SendToGuild.PerformAsync(new CommandBasedContext(Context), guildName, channelName, message, true, _database);
        }

        [Command("send_dm", RunMode = RunMode.Async)]
        public async Task SendDirectMessage(string userName, [Remainder] string message)
        {
            await Controllers.Messages.SendDirectMessage.PerformAsync(new CommandBasedContext(Context), userName, message, false, _database);
        }

        [Command("send_dm+r", RunMode = RunMode.Async)]
        public async Task SendDirectMessageRepliable(string userName, [Remainder] string message)
        {
            await Controllers.Messages.SendDirectMessage.PerformAsync(new CommandBasedContext(Context), userName, message, true, _database);
        }

        [Command("send_reply", RunMode = RunMode.Async)]
        public async Task SendReplyError(string key)
        {
            await Context.Channel.SendMessageAsync("Please specify a message, ex: `send_reply iMIb62udZ7R/KCfhn634+AHvrrQ Don't make a girl a promise you know you can't keep.`");
        }

        [Command("send_reply", RunMode = RunMode.Async)]
        public async Task SendReply(string key, [Remainder] string message)
        {
            await Controllers.Messages.SendReply.PerformAsync(new CommandBasedContext(Context), key, message, false,_database);
        }

        [Command("send_reply+r", RunMode = RunMode.Async)]
        public async Task SendReplyRepliableError(string key)
        {
            await Context.Channel.SendMessageAsync("Please specify a message, ex: `send_reply+r iMIb62udZ7R/KCfhn634+AHvrrQ Don't make a girl a promise you know you can't keep.`");
        }

        [Command("send_reply+r", RunMode = RunMode.Async)]
        public async Task SendReplyRepliable(string key, [Remainder] string message)
        {
            await Controllers.Messages.SendReply.PerformAsync(new CommandBasedContext(Context), key, message, true, _database);
        }
    }
}
