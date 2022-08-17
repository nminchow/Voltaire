using System.Threading.Tasks;
using Discord.Interactions;
using Discord;
using System.Linq;

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

                await Context.Interaction.RespondWithModalAsync<Views.Modals.MessagePrompt>($"send-message:{channelId},{repliable}", null, modifyModal: builder => {
                    builder.Title = prompt;
                });

            }
    }
}
