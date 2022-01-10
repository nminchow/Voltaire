using Discord.Commands;
using System.Linq;
using System;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Controllers.Reactions
{
    class React
    {
        public static async Task PerformAsync(UnifiedContext context, ulong messageId, string emoji, DataBase db)
        {
            var guildList = Send.GuildList(context);

            var task = await Task.WhenAll(guildList.SelectMany(x => x.TextChannels).Select(async x => {
                try {
                    var message = await x.GetMessageAsync(messageId);
                    return message;
                } catch {
                    return null;
                }
            }));

            var r = task.Where(x => x != null);

            if (r.Count() == 0) {
                await context.Channel.SendMessageAsync("message not found");
                return;
            }

            // test for simple emoji (😃)
            try {
                var d = new Discord.Emoji(emoji);
                await r.First().AddReactionAsync(d);
                await Send.SendSentEmoteIfCommand(context);
                return;
            } catch (Discord.Net.HttpException) {}

            // look for custom discord emotes
            var emote = guildList.SelectMany(x => x.Emotes).FirstOrDefault(x => $":{x.Name}:".IndexOf(
                emoji, StringComparison.OrdinalIgnoreCase) != -1);

            if (emote != null) {
                await r.First().AddReactionAsync(emote);
                await Send.SendSentEmoteIfCommand(context);
            } else {
                await context.Channel.SendMessageAsync("Emoji not found. To send a custom emote, use the emote's name.");
            }
            return;
        }
    }
}
