using Discord;
using Discord.WebSocket;
using groupOrdering.Boundary;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.UI
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private GroupBuyingApp _app;
        private CreateOrderUI _createOrderUI;
        private JoinOrderUI _joinOrderUI;
        private EditOrderUI _editOrderUI;
        private EndOrderUI _endOrderUI;
        private CreateStoreUI _createStoreUI;

        public async Task MainAsync()
        {
            var cinfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _app = new GroupBuyingApp();
            _client = new DiscordSocketClient(cinfig);

            _createOrderUI = new CreateOrderUI(_client, _app);
            _joinOrderUI = new JoinOrderUI(_client, _app);
            _editOrderUI = new EditOrderUI(_client, _app);
            _endOrderUI = new EndOrderUI(_client, _app);
            _createStoreUI = new CreateStoreUI(_client, _app);

            _client.Log += Log;

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
