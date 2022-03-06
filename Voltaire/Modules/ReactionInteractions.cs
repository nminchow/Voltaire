using System.Threading.Tasks;
using Discord.Interactions;
using System;


namespace Voltaire.Modules
{
    public class ReactionInteractions : InteractionsBase
    {
      public ReactionInteractions(DataBase database): base(database) {}

      // todo: make this a "message command"
      // https://discordnet.dev/guides/int_framework/intro.html#message-commands
      [SlashCommand("react", "add an anonymous reaction to a message")]
      public async Task React(
        [Summary("message-ID", "The discord ID of the message you'd like to react to")] string messageIdString,
        [Summary("emoji", "the emoji reaction you'd like to send")] string emoji
      )
      {
          ulong messageId;
          try {
            messageId = UInt64.Parse(messageIdString);
          } catch {
            await RespondAsync("Problem parsing message Id.", ephemeral: true);
            return;
          }
          await Controllers.Reactions.React.PerformAsync(new InteractionBasedContext(Context, Responder), messageId, emoji, _database);
      }
    }
}
