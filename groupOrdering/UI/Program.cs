using Discord;
using Discord.WebSocket;
using groupOrdering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.UI
{
    class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private GroupBuyingApp _app;

        public async Task MainAsync()
        {
            _app = new GroupBuyingApp();
            var cinfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            _client = new DiscordSocketClient(cinfig);
            _client.MessageReceived += CommandHandler;

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

        private Task CommandHandler(SocketMessage message)
        {
            string messageString = message.Content;

            if (!messageString.StartsWith('!'))
                return Task.CompletedTask;

            if (message.Author.IsBot)
                return Task.CompletedTask;

            string[] words = messageString.Split(' ');

            string command = words[0].Substring(1);

            switch (command)
            {
                case "hello":
                    message.Channel.SendMessageAsync($@"Hello {message.Author.Mention}");
                    break;
                case "CreateGroupBuying":
                    _app.GetCreateOrderHandler().CreateGroupBuying(message.Author.Id.ToString());
                    message.Channel.SendMessageAsync(_app.GetCreateOrderHandler().ListStore());
                    break;
                case "ChooseExistStore":
                    _app.GetCreateOrderHandler().ChooseExistStore(message.Author.Id.ToString(), words[1], message.Channel.Id.ToString());
                    message.Channel.SendMessageAsync("已選擇商家");
                    break;
                case "SetEndTime":
                    if (!_app.GetCreateOrderHandler().CheckChooseStore(message.Author.Id.ToString()))
                    {
                        message.Channel.SendMessageAsync("尚未挑選店家");
                    }
                    else if (!_app.GetCreateOrderHandler().CheckEndTime(message.Author.Id.ToString(), Convert.ToDateTime(words[1])))
                    {
                        message.Channel.SendMessageAsync("請確認時間格式是否正常");
                    }
                    else
                    {
                        _app.GetCreateOrderHandler().SetEndTime(message.Author.Id.ToString(), Convert.ToDateTime(words[1]));
                        message.Channel.SendMessageAsync("已設定團購結束時間");
                    }
                    break;
                case "EndEdit":
                    if (_app.GetCreateOrderHandler().CheckOrderValid(message.Author.Id.ToString()))
                    {
                        _app.GetCreateOrderHandler().EndEdit(message.Author.Id.ToString());
                        message.Channel.SendMessageAsync("已建立團購");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("尚未設定團購結束時間");
                    }
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
