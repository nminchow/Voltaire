using System;
using Discord.Interactions;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Modules
{
    public class MessageInteractions : InteractionsBase
    {
      public MessageInteractions(DataBase database): base(database) {}

      [SlashCommand("send", "Send an anonymous message to the specified channel")]
      public async Task Send(string channelName, string message)
      {
        try {
          await Controllers.Messages.Send.PerformAsync(new InteractionBasedContext(Context, Responder), channelName, message, false, _database);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
        }
      }

      // todo: handle this command in DMs
      [SlashCommand("volt", "Send an anonymous message to the current channel")]
      public async Task Volt(string message)
      {
        try {
          await SendToGuild.LookupAndSendAsync(Context.Guild, new InteractionBasedContext(Context, Responder), Context.Channel.Name, message, false, _database);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
        }
      }

    }

}