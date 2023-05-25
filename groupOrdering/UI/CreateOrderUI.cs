using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using groupOrdering.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace groupOrdering.UI
{
    public class CreateOrderUI
    {
        private readonly DiscordSocketClient _client;
        private readonly GroupBuyingApp _app;

        private const string TEST_SERVER_ID = "test";

        private const string CREATE_ORDER_COMMAND = "create-order";
        private const string END_TIME_MODAL_ID = "end-time-model";
        private const string END_TIME_BUTTON_ID = "end-time-button";
        private const string SEND_BUTTON_ID = "send-button";
        private const string STORE_MENU_ID = "store-menu";

        public CreateOrderUI(DiscordSocketClient client, GroupBuyingApp app) 
        {
            _client = client;
            _app = app;

            _client.Ready += CreateOrder_Ready;
            _client.SlashCommandExecuted += CreateOrderCommandHandler;
            _client.SelectMenuExecuted += StoreMenuHandler;
            _client.ButtonExecuted += EndDateTimeButtonHandler;
            _client.ButtonExecuted += SendButtonHandler;
            _client.ModalSubmitted += EndTimeDatePickerHandler;
        }

        private async Task CreateOrder_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(CREATE_ORDER_COMMAND)
                .WithDescription("創建一筆團購訂單")
                .AddOption("團購名稱", ApplicationCommandOptionType.String, "請輸入欲創建的團購名稱", isRequired: true);
            try
            {
                // 測試時只允許特定伺服器啟用指令，正式時使用下面那個
#if DEBUG
                await guild.CreateApplicationCommandAsync(createOrderCommand.Build());
#else
                await _client.CreateGlobalApplicationCommandAsync(createOrderCommand.Build());
#endif
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    
        private async Task CreateOrderCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != CREATE_ORDER_COMMAND)
                return;

            string userID = command.User.Id.ToString();
            User user = new User(userID);
            string serverID;
#if DEBUG
            serverID = TEST_SERVER_ID;
#else
            serverID = command.ChannelId.ToString() ?? "";
#endif
            string groupBuyingName = string.Join(", ", command.Data.Options.First().Value);
            _app.GetCreateOrderHandler().CreateGroupBuying(user, groupBuyingName, serverID);

            List<Store> stores = _app.GetCreateOrderHandler().ListStore(serverID);

            SelectMenuBuilder menu = BuildStoreMenu(stores);
            ButtonBuilder endTimeButton = BuildEndDateTimeButton();
            ButtonBuilder sendButton = BuildSendButton();

            ComponentBuilder messageBuilder = new ComponentBuilder()
                .WithSelectMenu(menu)
                .WithButton(endTimeButton)
                .WithButton(sendButton);

            await command.RespondAsync(components: messageBuilder.Build());
        }

        private SelectMenuBuilder BuildStoreMenu(List<Store> stores)
        {
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("請選擇店家")
                .WithCustomId(STORE_MENU_ID);
            foreach (Store store in stores)
            {
                menuBuilder.AddOption($"{store.StoreID},   {store.StoreName}", $"{store.StoreID}-{store.StoreName}");
            }
            return menuBuilder;
        }

        private async Task StoreMenuHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != STORE_MENU_ID)
                return;
            string[] value = string.Join(", ", messageComponent.Data.Values).Split('-');
            string storeID = value[0];
            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            string serverID;
#if DEBUG
            serverID = TEST_SERVER_ID;
#else
            serverID = messageComponent.ChannelId.ToString() ?? "";
#endif
            _app.GetCreateOrderHandler().ChooseExistStore(user, storeID, serverID);
            await messageComponent.DeferAsync();
        }
        
        private ButtonBuilder BuildEndDateTimeButton()
        {
            var buttonBuilder = new ButtonBuilder()
                .WithLabel("點此設定截止時間")
                .WithCustomId(END_TIME_BUTTON_ID)
                .WithStyle(ButtonStyle.Primary);
            return buttonBuilder;
        }

        private async Task EndDateTimeButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != END_TIME_BUTTON_ID)
                return;
            Modal endDate = BuildEndTimeDatePicker();
            await messageComponent.RespondWithModalAsync(endDate);
        }

        private ButtonBuilder BuildSendButton()
        {
            var buttonBuilder = new ButtonBuilder()
                .WithLabel("送出")
                .WithCustomId(SEND_BUTTON_ID)
                .WithStyle(ButtonStyle.Primary);
            return buttonBuilder;
        }

        private async Task SendButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != SEND_BUTTON_ID)
                return;

            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);

            bool isChoseEndTime = _app.GetCreateOrderHandler().CheckEndTimeValid(user);
            bool isSetStore = _app.GetCreateOrderHandler().CheckChooseStore(user);
            if (!isSetStore)
            {
                await messageComponent.RespondAsync("請設置商店");
                return;
            }
            if (!isChoseEndTime)
            {
                await messageComponent.RespondAsync("請設置截止時間");
                return;
            }
            _app.GetCreateOrderHandler().EndEdit(user);
            await messageComponent.Message.DeleteAsync();
            await messageComponent.Channel.SendMessageAsync("已建立團購");
        }

        private Modal BuildEndTimeDatePicker()
        {
            var modalBuilder = new ModalBuilder()
                .WithTitle("請選擇截止時間")
                .WithCustomId(END_TIME_MODAL_ID)
                .AddTextInput("Year", "year", placeholder: "yyyy", required: true, maxLength: 4)
                .AddTextInput("Month", "month", placeholder: "MM", required: true, maxLength: 2)
                .AddTextInput("Day", "day", placeholder: "dd", required: true, maxLength: 2)
                .AddTextInput("Hour", "hour", placeholder: "hh", required: true, maxLength: 2)
                .AddTextInput("Minutes", "minutes", placeholder: "mm", required: true, maxLength: 2);
            return modalBuilder.Build();
        }

        private async Task EndTimeDatePickerHandler(SocketModal modal)
        {
            if (modal.Data.CustomId != END_TIME_MODAL_ID)
                return;
            List<SocketMessageComponentData> components = modal.Data.Components.ToList();
            try
            {
                int year = Int32.Parse(components.First(x => x.CustomId == "year").Value);
                int month = Int32.Parse(components.First(x => x.CustomId == "month").Value);
                int day = Int32.Parse(components.First(x => x.CustomId == "day").Value);
                int hour = Int32.Parse(components.First(x => x.CustomId == "hour").Value);
                int minutes = Int32.Parse(components.First(x => x.CustomId == "minutes").Value);
                DateTime endTime = new DateTime(year, month, day, hour, minutes, 0);
                if (!_app.GetCreateOrderHandler().CheckEndTime(endTime))
                {
                    await modal.RespondAsync("請設置現在以後的時間");
                }
                else
                {
                    string userID = modal.User.Id.ToString();
                    User user = new User(userID);
                    _app.GetCreateOrderHandler().SetEndTime(user, endTime);
                    
                    await modal.RespondAsync($"設定團購結束時間為{endTime}");
                }
            }
            catch
            {
                await modal.RespondAsync("請確認時間格式是否正確");
            }
        }
    }
}
