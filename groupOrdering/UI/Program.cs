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

            string userID = message.Author.Id.ToString();
            User user = new User(userID);
            switch (command)
            {
                case "hello":
                    message.Channel.SendMessageAsync($@"Hello {message.Author.Mention}");
                    break;
                case "CreateGroupBuying":
                    _app.GetCreateOrderHandler().CreateGroupBuying(user);
                    //TODO serverID被固定住
                    string serverID = "test";
                    message.Channel.SendMessageAsync(_app.GetCreateOrderHandler().ListStore(serverID));
                    break;
                case "ChooseExistStore":
                    if (!_app.GetCreateOrderHandler().CheckStartOrder(user))
                    {
                        message.Channel.SendMessageAsync("尚未建立團購");
                    }
                    else
                    {
                        //TODO ServerID要修正
                        _app.GetCreateOrderHandler().ChooseExistStore(user, words[1], "test");
                        message.Channel.SendMessageAsync("已選擇商家");
                    }
                    break;
                case "SetEndTime":
                    if (!_app.GetCreateOrderHandler().CheckChooseStore(user))
                    {
                        message.Channel.SendMessageAsync("尚未挑選店家");
                    }
                    else if (!_app.GetCreateOrderHandler().CheckEndTime(Convert.ToDateTime(words[1])))
                    {
                        message.Channel.SendMessageAsync("請確認時間格式是否正常");
                    }
                    else
                    {
                        _app.GetCreateOrderHandler().SetEndTime(user, Convert.ToDateTime(words[1]));
                        message.Channel.SendMessageAsync("已設定團購結束時間");
                    }
                    break;
                case "EndEdit":
                    if (_app.GetCreateOrderHandler().CheckEndTimeValid(user))
                    {
                        _app.GetCreateOrderHandler().EndEdit(user);
                        message.Channel.SendMessageAsync("已建立團購");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("尚未設定團購結束時間");
                    }
                    break;
                case "ListAllOrder":
                    //TODO serverID被固定住
                    serverID = "test";
                    List<GroupBuying> groupBuyingData = _app.GetJoinOrderHandler().ListAllOrder(serverID);
                    string output = "團購編號\t店家編號\t團購店家名稱\n";
                    for(int i = 0; i < groupBuyingData.Count; i++)
                    {
                        output += $"{groupBuyingData[i].GroupBuyingID}\t{groupBuyingData[i].storeID}\t{groupBuyingData[i].getGroupbuyingName()}\n";
                    }
                    message.Channel.SendMessageAsync(output);
                    break;
                case "ListItemOfStore":
                    //TODO serverID被固定住
                    serverID = "test";
                    List<StoreItem> storeItems = _app.GetJoinOrderHandler().ListItemsOfStore(words[1], serverID);
                    output = "餐點編號\t餐點名稱\t餐點價格\n";
                    for (int i = 0; i < storeItems.Count; i++)
                    {
                        output += $"{storeItems[i].storeitemID}\t{storeItems[i].storeitemName}\t{storeItems[i].storeitemPrice}元\n";
                    }
                    message.Channel.SendMessageAsync(output);
                    break;
                case "JoinOrder":
                    //TODO serverID被固定住
                    serverID = "test";
                    if (_app.GetJoinOrderHandler().JoinOrder(serverID, words[1], user))
                    {
                        message.Channel.SendMessageAsync($"已加入團購 {words[2]}");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("加入團購失敗");
                    }
                    break;
                case "AddItem":
                    if (_app.GetJoinOrderHandler().AddItem(user, words[1], Int32.Parse(words[2])))
                    {
                        message.Channel.SendMessageAsync("新增餐點成功");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("新增餐點失敗");
                    }
                    break;
                case "EditItem":
                    if (_app.GetJoinOrderHandler().EditItem(user, words[1], Int32.Parse(words[2])))
                    {
                        message.Channel.SendMessageAsync("修改餐點成功");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("修改餐點失敗");
                    }
                    break;
                case "DeleteItem":
                    if (_app.GetJoinOrderHandler().DeleteItem(user, words[1]))
                    {
                        message.Channel.SendMessageAsync("刪除餐點成功");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("刪除餐點失敗");
                    }
                    break;
                case "SubmitOrder":
                    if (_app.GetJoinOrderHandler().CheckValid(user))
                    {
                        message.Channel.SendMessageAsync($"已完成團購點餐\n總共花費:{_app.GetJoinOrderHandler().SubmitOrder(user)}元");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync("請先加入團購");
                    }
                    break;
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
