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
        // this is broken in DMs because of this: https://github.com/discord/discord-api-docs/issues/2820
        try {
          await Controllers.Messages.SendToGuild.SendToChannelById(channel.Id, new InteractionBasedContext(Context, Responder), message, repliable, _database);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
        }
      }

      [SlashCommand("volt", "send an anonymous message to the current channel")]
      public async Task Volt(string message, bool repliable = false)
      {
        if (Context.Guild == null) {
          var function = Controllers.Messages.Send.SendMessageToChannel(Context.Channel, repliable == true, new InteractionBasedContext(Context, Responder), false);
          await function("", message);
          await RespondAsync("Sent!", ephemeral: true);
          return;
        }
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