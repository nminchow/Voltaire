using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Voltaire
{
    class Program
    {
        private CommandService _commands;
        private DiscordShardedClient _client;
        private IServiceProvider _services;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {

            IConfiguration configuration = LoadConfig.Instance.config;
            var db = new DataBase(configuration.GetConnectionString("sql"));

            _client = new DiscordShardedClient();
            _client.Log += Log;
            _client.JoinedGuild += Controllers.Helpers.JoinedGuild.Joined(db, configuration["discordBotListToken"]);
            // disable joined message for now
            //_client.UserJoined += Controllers.Helpers.UserJoined.SendJoinedMessage;


            _commands = new CommandService();

            string token = configuration["discordAppToken"];

            StripeConfiguration.SetApiKey(configuration["stripe_api_key"]);

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(db)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.SetGameAsync("!volt help");

            await _client.StartAsync();


            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommandAsync;
            _client.ReactionAdded += HandleReaction;
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
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

        private async Task HandleReaction(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
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
                await channel.SendMessageAsync("Error deleting message. Does the bot have needed permission?");
            }
            return;
        }


        private async Task SendCommandAsync(ShardedCommandContext context, int argPos)
        {
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await Controllers.Messages.Send.SendErrorWithDeleteReaction(context, result.ErrorReason);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.FromResult(0);
        }
    }
}
