using System.Threading.Tasks;
using Discord.Interactions;
using System.Linq;
using Discord.WebSocket;
using System;

namespace Voltaire.Modules
{
  public static class MessageCommand
  {
      public async static Task MessageCommandHandler(SocketMessageCommand arg)
      {
          var channel = arg.Channel as Discord.ITextChannel;
          await channel.CreateThreadAsync("test", message: arg.Data.Message);
          await arg.RespondAsync("Thread Created!", ephemeral: true);
      }
  }
}