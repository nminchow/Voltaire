using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

namespace Voltaire
{
  public abstract class UnifiedContext
  {

    public DiscordShardedClient Client { get; set; }
    public IGuild Guild { get; set; }
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
    public InteractionBasedContext(ShardedInteractionContext context)
    {
      Client = context.Client;
      Guild = context.Guild;
      Channel = context.Channel;
      User = context.User;
    }
  }

}
