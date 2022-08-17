using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Stripe;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Voltaire
{
    class Program
    {
        private CommandService _commands;
        private InteractionService _interactions;
        private DiscordShardedClient _client;
        private IServiceProvider _services;

        private System.Collections.Generic.IEnumerable<Discord.Interactions.ModuleInfo> _interactionModules;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {

            IConfiguration configuration = LoadConfig.Instance.config;
            var optionsBuilder = new DbContextOptionsBuilder<Voltaire.DataBase>();
            optionsBuilder.UseSqlServer($@"{configuration.GetConnectionString("sql")}");
            var db = new DataBase(optionsBuilder.Options);

            var config = new DiscordSocketConfig {
                // LogLevel = LogSeverity.Debug,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.GuildMembers |
                   GatewayIntents.Guilds |
                   GatewayIntents.GuildEmojis |
                   GatewayIntents.GuildMessages |
                   GatewayIntents.GuildMessageReactions |
                   GatewayIntents.DirectMessages |
                   GatewayIntents.DirectMessageReactions
            };

            _client = new DiscordShardedClient(config);
            _client.Log += Log;
            _client.JoinedGuild += Controllers.Helpers.JoinedGuild.Joined(db);
            // disable joined message for now
            //_client.UserJoined += Controllers.Helpers.UserJoined.SendJoinedMessage;


            _commands = new CommandService();
            _interactions = new InteractionService(_client);

            string token = configuration["discordAppToken"];

            StripeConfiguration.SetApiKey(configuration["stripe_api_key"]);

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(_interactions)
                .AddDbContext<DataBase>(options => options.UseSqlServer($@"{configuration.GetConnectionString("sql")}"))
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.SetGameAsync("/volt-help", null, ActivityType.Watching);

            await _client.StartAsync();


            await Task.Delay(-1);

        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReaction;
            _client.ShardReady += RegisterCommands;
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _interactionModules = await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _interactions.SlashCommandExecuted += SlashCommandExecuted;
            _client.InteractionCreated += HandleInteraction;
        }

        private async Task RegisterCommands(DiscordSocketClient client)
        {
            if (client.ShardId != 0) return;
            if (LoadConfig.Instance.config["dev_server"] != null) {
                // await _interactions.AddModulesGloballyAsync(true, new Discord.Interactions.ModuleInfo[] {});
                await _interactions.AddModulesToGuildAsync(
                    client.Guilds.First(x => x.Id.ToString() == LoadConfig.Instance.config["dev_server"]),
                    true,
                    _interactionModules.ToArray()
                );
                return;
            }
            await _interactions.AddModulesGloballyAsync(true, _interactionModules.ToArray());
        }

        async Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await arg2.Interaction.RespondAsync($"Unmet Precondition: {arg3.ErrorReason}", ephemeral: true);
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await arg2.Interaction.RespondAsync("Unknown command", ephemeral: true);
                        break;
                    case InteractionCommandError.BadArgs:
                        await arg2.Interaction.RespondAsync("Invalid number or arguments", ephemeral: true);
                        break;
                    case InteractionCommandError.Exception:
                        Console.WriteLine("Command Error:");
                        Console.WriteLine(arg3.ErrorReason);
                        await arg2.Interaction.RespondAsync($"Command exception: {arg3.ErrorReason}. If this message persists, please let us know in the support server (https://discord.gg/xyzMyJH) !", ephemeral: true);
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await arg2.Interaction.RespondAsync("Command could not be executed", ephemeral: true);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            //var context = new ShardedCommandContext(_client, message);
            var context = new ShardedCommandContext(_client, message);

            // Create a number to track where the prefix ends and the command begins
            var prefix = $"!volt ";
            int argPos = prefix.Length - 1;

            // short circut DMs
            if (context.IsPrivate && !context.User.IsBot && !(message.HasStringPrefix(prefix, ref argPos)))
            {
                await SendCommandAsync(context, 0);
                Console.WriteLine("processed message!");
                return;
            }

            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            // quick logging
            Console.WriteLine("processed message!");
            // Execute the command. (result does not indicate a return value,
            // rather an object stating if the command executed successfully)
            await SendCommandAsync(context, argPos);
        }

        private async Task HandleInteraction (SocketInteraction arg)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new ShardedInteractionContext(_client, arg);
                await _interactions.ExecuteCommandAsync(ctx, _services);
                Console.WriteLine("processed interaction!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught exception:");
                Console.WriteLine(ex);

                // If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if(arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }

        private async Task HandleReaction(Cacheable<IUserMessage, ulong> cache, Cacheable<IMessageChannel, ulong> channelCache, SocketReaction reaction)
        {
            if (reaction.Emote.Name != Controllers.Messages.Send.DeleteEmote)
            {
                return;
            }

            try
            {
                var message = await cache.GetOrDownloadAsync();
                if (!message.Reactions[reaction.Emote].IsMe || message.Reactions[reaction.Emote].ReactionCount == 1)
                {
                    return;
                }
                Console.WriteLine("processed reaction!");
                await  message.DeleteAsync();
            }
            catch (Exception e)
            {
                await channelCache.Value.SendMessageAsync("Error deleting message. Does the bot have needed permission?");
            }
            return;
        }


        private async Task SendCommandAsync(ShardedCommandContext context, int argPos)
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await Controllers.Messages.Send.SendErrorWithDeleteReaction(new CommandBasedContext(context), result.ErrorReason);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.FromResult(0);
        }
    }
}
