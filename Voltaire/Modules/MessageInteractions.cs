using Discord.Interactions;
using System.Threading.Tasks;


namespace Voltaire.Modules
{
    public class MessageInteractions : InteractionModuleBase
    {

      [SlashCommand("echo", "Echo an input")]
      public async Task Echo(string input)
      {
          await ReplyAsync("test");
          await RespondAsync(input);
      }

    }

}