using System;
using Discord.Interactions;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Modules
{
    public class MessageInteractions : InteractionModuleBase<ShardedInteractionContext>
    {
        private DataBase _database;

        public MessageInteractions(DataBase database)
        {
            _database = database;
        }


      [SlashCommand("echo", "Echo an input")]
      public async Task Echo(string input)
      {
        // await ReplyAsync("test");
        // Context.Channel.SendMessageAsync("yo");
        try {
          await SendToGuild.LookupAndSendAsync(Context.Guild, new InteractionBasedContext(Context), Context.Channel.Name, input, false, _database);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
        }
        // TODO : figure out if we can silently acknowledge
        await RespondAsync();
      }

    }

}