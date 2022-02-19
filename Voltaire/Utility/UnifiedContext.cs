using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;


namespace Voltaire
{
  public abstract class UnifiedContext
  {

    public DiscordShardedClient Client { get; set; }
    public SocketGuild Guild { get; set; }
    public IMessageChannel Channel { get; set; }
    public IUser User { get;  set; }
  }


  public class CommandBasedContext : UnifiedContext
  {

    public CommandBasedContext(ShardedCommandContext context)
    {
      Client = context.Client;
      Guild = context.Guild;
      Channel = context.Channel;
      User = context.User;
      Message = context.Message;
    }

    public IUserMessage Message { get; set; }

  }

  public class InteractionBasedContext: UnifiedContext
  {
    public InteractionBasedContext(ShardedInteractionContext context, Func<string, Discord.Embed, Task> responder)
    {
      Client = context.Client;
      Guild = context.Guild;
      Channel = context.Channel;
      User = context.User;
      Responder = responder;
    }

    public Func<string, Discord.Embed, Task> Responder { get; set; }
  }

}
