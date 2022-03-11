using System;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using Voltaire.Controllers.Messages;

namespace Voltaire.Modules
{
    public class MessageInteractions : InteractionsBase
    {
      public MessageInteractions(DataBase database): base(database) {}

      [SlashCommand("send", "send an anonymous message to the specified channel in the current server")]
      public async Task Send(SocketChannel channel, string message, bool repliable = false)
      {
        try {
          await Controllers.Messages.SendToGuild.LookupAndSendAsync(Context.Guild, new InteractionBasedContext(Context, Responder), channel.Id.ToString(), message, repliable, _database);
          // await Controllers.Messages.Send.PerformAsync(new InteractionBasedContext(Context, Responder), channel.Id.ToString(), message, false, _database);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
        }
      }

      // todo: handle this command in DMs
      [SlashCommand("volt", "send an anonymous message to the current channel")]
      public async Task Volt(string message, bool repliable = false)
      {
        try {
          await SendToGuild.LookupAndSendAsync(Context.Guild, new InteractionBasedContext(Context, Responder), Context.Channel.Name, message, repliable, _database);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
        }
      }

      [SlashCommand("send-dm", "send an anonymous message to the specified user")]
      public async Task SendDirectMessage(SocketUser user, string message, bool repliable = false)
      {
        await Controllers.Messages.SendDirectMessage.PerformAsync(new InteractionBasedContext(Context, Responder), user.Id.ToString(), message, repliable, _database);
      }

      [SlashCommand("send-reply", "reply to an anonymous message with a reply code")]
      public async Task SendReply([Summary("reply-code", "the code on the message you'd like to reply to")] string reply_code, string message, bool repliable = false)
      {
        await Controllers.Messages.SendReply.PerformAsync(new InteractionBasedContext(Context, Responder), reply_code, message, repliable, _database);
      }

    }

}