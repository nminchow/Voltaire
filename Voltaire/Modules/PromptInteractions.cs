using System.Threading.Tasks;
using Discord.Interactions;
using System.Linq;
using Discord.WebSocket;

namespace Voltaire.Modules
{
    public class PromptInteractions : InteractionsBase
    {

        public PromptInteractions(DataBase database): base(database) {}

            [ComponentInteraction("prompt-message:*,*")]
            public async Task PromptUserForInput(string channelId, string repliable) {

                var channel = Context.Guild.TextChannels.Where(x => x.Id.ToString() == channelId).FirstOrDefault();

                if (channel == null) {
                    await RespondAsync("It looks like this prompt is no longer working. Please contact your admin!", ephemeral: true);
                    return;
                }

                var prompt = $"Anonymous message to #{channel}";

                var truncatedPrompt = prompt.Length > 45 ? $"{prompt.Substring(0, 42)}..." : prompt;

                await Context.Interaction.RespondWithModalAsync<Views.Modals.MessagePrompt>($"send-message:{channelId},{repliable}", null, modifyModal: builder => {
                    builder.Title = truncatedPrompt;
                });

            }

            [ComponentInteraction("prompt-reply:*:*")]
            public async Task PromptUserForReploy(string replyHash, string replyable) {

                var originalInteraction = Context.Interaction as SocketMessageComponent;
                var author = originalInteraction.Message.Embeds.First().Author.ToString();

                if (author.EndsWith(" replied")) {
                    author = author.Remove(author.Length - 8);
                }

                var prompt = $"Anonymous reply to {author}";

                if (string.IsNullOrEmpty(author)) {
                    prompt = "Send Anonymous Reply";
                }

                await Context.Interaction.RespondWithModalAsync<Views.Modals.MessagePrompt>($"send-reply:{replyHash}:::{replyable}", null, modifyModal: builder => {
                    builder.Title = prompt;
                });

            }
    }
}
