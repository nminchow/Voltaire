using System;
using Discord.Interactions;
using System.Threading.Tasks;

namespace Voltaire.Modules
{
    public abstract class InteractionsBase : InteractionModuleBase<ShardedInteractionContext>
    {
        protected DataBase _database;
        protected Func<string, Discord.Embed, Task> Responder;
        protected Func<string, Discord.Embed, Task> PublicResponder;

        public InteractionsBase(DataBase database)
        {
            _database = database;
            Responder = (response, embed) =>
            {
                if (embed != null)  {
                    return RespondAsync(response, new[] { embed }, ephemeral: true);
                }
                return RespondAsync(response, ephemeral: true);
            };

            PublicResponder = (response, embed) =>
            {
                if (embed != null)  {
                    return RespondAsync(response, new[] { embed }, ephemeral: false);
                }
                return RespondAsync(response, ephemeral: false);
            };
        }
    }
}
