﻿using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using groupOrdering.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace groupOrdering.UI
{
    public class CreateOrderUI
    {
        private readonly DiscordSocketClient _client;
        private readonly CreateOrderHandler _handler;

        private readonly Dictionary<string, int> _storeBrowserCurrentNum;

        private const string LIST_STORE_COMMAND = "list-store";

        private const string CREATE_ORDER_MODAL_ID = "create-order-model";

        private const string NEXT_ITEM_BUTTON_ID = "store-browser-next-button";
        private const string PREVIOUS_ITEM_BUTTON_ID = "store-browser-previous-button";
        private const string CHOOSE_STORE_BUTTON_ID = "store-browser-choose-store-button";

        public CreateOrderUI(DiscordSocketClient client, GroupBuyingApp app) 
        {
            _client = client;
            _handler = app.GetCreateOrderHandler();

            _storeBrowserCurrentNum = new Dictionary<string, int>();

            _client.Ready += CreateOrder_Ready;
            _client.SlashCommandExecuted += ListStoreCommandHandler;
            _client.ModalSubmitted += CreateOrderModalHandler;
            _client.ButtonExecuted += ChooseStoreButtonHandler;
            _client.ButtonExecuted += NextStoreButtonHandler;
            _client.ButtonExecuted += PreviousStoreButtonHandler;
        }

        private async Task CreateOrder_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(LIST_STORE_COMMAND)
                .WithDescription("瀏覽所有店家");
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
    
        private async Task ListStoreCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != LIST_STORE_COMMAND)
                return;

            string userID = command.User.Id.ToString();
            string serverID = command.GuildId.ToString() ?? "";
            List<Store> stores = _handler.ListStore(serverID);

            _storeBrowserCurrentNum[userID] = 0;
            int num = _storeBrowserCurrentNum[userID] + 1;
            Embed storeInfo = BuildStoreEmbed(stores[_storeBrowserCurrentNum[userID]], num, stores.Count);
            ComponentBuilder componentBuilder = BuildStoreBrowser();
            await command.RespondAsync(embed: storeInfo, components: componentBuilder.Build(), ephemeral: true);
        }

        private ComponentBuilder BuildStoreBrowser()
        {
            var previousButtonBuilder = new ButtonBuilder()
                .WithEmote(new Emoji("◀"))
                .WithCustomId(PREVIOUS_ITEM_BUTTON_ID)
                .WithStyle(ButtonStyle.Secondary);
            var chooseStoreButtonBuilder = new ButtonBuilder()
                .WithLabel("以此店家建立團購")
                .WithCustomId(CHOOSE_STORE_BUTTON_ID)
                .WithStyle(ButtonStyle.Primary);
            var nextButtonBuilder = new ButtonBuilder()
                .WithEmote(new Emoji("▶"))
                .WithCustomId(NEXT_ITEM_BUTTON_ID)
                .WithStyle(ButtonStyle.Secondary);
            var builder = new ComponentBuilder();
            builder.AddRow(new ActionRowBuilder().WithButton(previousButtonBuilder).WithButton(chooseStoreButtonBuilder).WithButton(nextButtonBuilder));
            return builder;
        }

        private Embed BuildStoreEmbed(Store store, int num, int total)
        {
            EmbedBuilder builder = new EmbedBuilder()
                .WithTitle(store.StoreName)
                .WithDescription(ListStoreItems(store.ListItemsOfStore()))
                .WithFooter($"頁{num}/{total}");
            return builder.Build();
        }

        private string ListStoreItems(List<StoreItem> items)
        {
            
            string result = "";
            foreach (var item in items)
            {
                string itemInfo = item.storeitemName;
                itemInfo = itemInfo.PadRight(10, '﹒');
                itemInfo += $"{item.storeitemPrice}元\n";
                result += itemInfo;
            }
            return result;
        }

        private async Task NextStoreButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != NEXT_ITEM_BUTTON_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            string serverID = messageComponent.GuildId.ToString() ?? "";
            var stores = _handler.ListStore(serverID);
            int num = _storeBrowserCurrentNum.GetValueOrDefault(userID, 0) + 1;
            if (num >= stores.Count)
            {
                await messageComponent.DeferAsync();
                return;
            }
            _storeBrowserCurrentNum[userID] = num;
            Embed storeInfo = BuildStoreEmbed(stores[num], num + 1, stores.Count);
            await messageComponent.UpdateAsync(x => x.Embed = storeInfo);
        }

        private async Task PreviousStoreButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != PREVIOUS_ITEM_BUTTON_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            string serverID = messageComponent.GuildId.ToString() ?? "";
            var stores = _handler.ListStore(serverID);
            int num = _storeBrowserCurrentNum.GetValueOrDefault(userID, 0) - 1;
            if (num < 0)
            {
                await messageComponent.DeferAsync();
                return;
            }
            _storeBrowserCurrentNum[userID] = num;
            Embed storeInfo = BuildStoreEmbed(stores[num], num + 1, stores.Count);
            await messageComponent.UpdateAsync(x => x.Embed = storeInfo);
        }

        private async Task ChooseStoreButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != CHOOSE_STORE_BUTTON_ID)
                return;

            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            string serverID = messageComponent.GuildId.ToString() ?? "";

            _handler.CreateGroupBuying(user, "", serverID);

            int num = _storeBrowserCurrentNum.GetValueOrDefault(user.UserID, 0);
            var storeList = _handler.ListStore(serverID);
            _handler.ChooseExistStore(user, storeList[num].StoreID, serverID);

            var createOrderModalBuilder = new ModalBuilder()
                .WithTitle("請選擇截止時間")
                .WithCustomId(CREATE_ORDER_MODAL_ID)
                .AddTextInput("團購名稱", "group-buying-name", required: true, maxLength: 45)
                .AddTextInput("Year", "year", placeholder: "yyyy", required: true, maxLength: 4, minLength: 1)
                .AddTextInput("Month", "month", placeholder: "MM", required: true, maxLength: 2, minLength: 1)
                .AddTextInput("Day", "day", placeholder: "dd", required: true, maxLength: 2, minLength: 1)
                .AddTextInput("Hour", "hour", placeholder: "hh", required: true, maxLength: 2, minLength: 1);

            await messageComponent.RespondWithModalAsync(createOrderModalBuilder.Build());
        }

        private async Task CreateOrderModalHandler(SocketModal modal)
        {
            if (modal.Data.CustomId != CREATE_ORDER_MODAL_ID)
                return;
            List<SocketMessageComponentData> components = modal.Data.Components.ToList();
            try
            {
                string groupBuyingName = components.First(x => x.CustomId == "group-buying-name").Value;
                int year = Int32.Parse(components.First(x => x.CustomId == "year").Value);
                int month = Int32.Parse(components.First(x => x.CustomId == "month").Value);
                int day = Int32.Parse(components.First(x => x.CustomId == "day").Value);
                int hour = Int32.Parse(components.First(x => x.CustomId == "hour").Value);
                DateTime endTime = new DateTime(year, month, day, hour, 0, 0);
                if (!_handler.CheckEndTime(endTime))
                {
                    await modal.RespondAsync("請設置現在以後的時間", ephemeral: true);
                }
                else
                {
                    string userID = modal.User.Id.ToString();
                    User user = new User(userID);
                    _handler.SetGroupBuyingName(user, groupBuyingName);
                    _handler.SetEndTime(user, endTime);
                    _handler.EndEdit(user);
                    await modal.RespondAsync($"已建立團購「{groupBuyingName}」", ephemeral: true);
                }
            }
            catch(FormatException)
            {
                await modal.RespondAsync("請確認時間格式是否正確", ephemeral: true);
            }
            catch(Exception)
            {
                await modal.RespondAsync("發生錯誤", ephemeral: true);
            }
        }
    }
}
