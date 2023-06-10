using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using groupOrdering.Boundary;
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
    public class JoinOrderUI
    {
        private readonly DiscordSocketClient _client;
        private readonly GroupBuyingApp _app;
        private readonly Dictionary<string, JoinOrderUIData> _joinOrderUIData;

        private const string LIST_ALL_ORDER_COMMAND = "list-all-order";
        private const string LIST_ITEM_OF_STORE_COMMAND = "list-item-of-store";
        private const string JOIN_ORDER_COMMAND = "join-order";
        private const string OPERATOR_TYPE_ID = "join-operator-type";
        private const string STORE_ITEM_ID = "join-store-item";
        private const string STORE_ITEM_QUANTITY_ID = "join-store-item-quantity";
        private const string EDIT_ITEM_BTN_ID = "join-edit-item-btn";
        private const string SUBMIT_ORDER_BTN_ID = "submit-order-btn";

        public JoinOrderUI(DiscordSocketClient client, GroupBuyingApp app)
        {
            _client = client;
            _app = app;
            _joinOrderUIData = new Dictionary<string, JoinOrderUIData>();
            _client.Ready += ListAllOrder_Ready;
            _client.Ready += ListItemOfStore_Ready;
            _client.Ready += JoinOrder_Ready;
            _client.SlashCommandExecuted += ListAllOrderCommandHandler;
            _client.SlashCommandExecuted += ListItemOfStoreCommandHandler;
            _client.SlashCommandExecuted += JoinOrderCommandHandler;
            _client.SelectMenuExecuted += OperatorMenuHandler;
            _client.SelectMenuExecuted += StoreItemMenuHandler;
            _client.SelectMenuExecuted += StoreQuantityMenuHandler;
            _client.ButtonExecuted += EditOrderButtonHandler;
            _client.ButtonExecuted += SubmitOrderButtonHandler;
        }

        private async Task ListAllOrder_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(LIST_ALL_ORDER_COMMAND)
                .WithDescription("查看開放中的團購訂單");
            try
            {
                // 測試時只允許特定伺服器啟用指令，正式時使用下面那個
                await guild.CreateApplicationCommandAsync(createOrderCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private async Task ListItemOfStore_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(LIST_ITEM_OF_STORE_COMMAND)
                .WithDescription("查看團購的商店菜單")
                .AddOption("商店編號", ApplicationCommandOptionType.String, "請輸入欲查看的商店編號", isRequired: true);
            try
            {
                // 測試時只允許特定伺服器啟用指令，正式時使用下面那個
                await guild.CreateApplicationCommandAsync(createOrderCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private async Task JoinOrder_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(JOIN_ORDER_COMMAND)
                .WithDescription("加入團購訂單")
                .AddOption("團購編號", ApplicationCommandOptionType.String, "請輸入欲加入的團購編號", isRequired: true);
            try
            {
                // 測試時只允許特定伺服器啟用指令，正式時使用下面那個
                await guild.CreateApplicationCommandAsync(createOrderCommand.Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private async Task ListAllOrderCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != LIST_ALL_ORDER_COMMAND)
                return;

            string serverID = command.GuildId.ToString() ?? "";

            List<GroupBuying> groupBuyings = _app.GetJoinOrderHandler().ListAllOrder(serverID);

            string allorders = "團購編號\t店家編號\t團購店家名稱\n";
            for (int i = 0; i < groupBuyings.Count; i++)
            {
                allorders += $"{groupBuyings[i].GroupBuyingID}\t{groupBuyings[i].StoreID}\t{groupBuyings[i].GroupBuyingName}\n";
            }

            await command.RespondAsync(text : allorders, ephemeral: true);
        }

        private async Task ListItemOfStoreCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != LIST_ITEM_OF_STORE_COMMAND)
                return;

            string serverID = command.GuildId.ToString() ?? "";

            string storeID = string.Join(", ", command.Data.Options.First().Value);
            List<StoreItem> storeItems = _app.GetJoinOrderHandler().ListItemsOfStore(storeID, serverID, new StoresBoundary());

            string allitems = "餐點編號\t餐點名稱\t餐點價格\n";
            for (int i = 0; i < storeItems.Count; i++)
            {
                allitems += $"{storeItems[i].StoreitemID}\t{storeItems[i].StoreitemName}\t{storeItems[i].StoreitemPrice}\n";
            }

            await command.RespondAsync(text: allitems, ephemeral: true);
        }

        private async Task JoinOrderCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != JOIN_ORDER_COMMAND)
                return;

            string userID = command.User.Id.ToString();
            User user = new User(userID);
            string serverID = command.GuildId.ToString() ?? "";
            string groupbuyingID = string.Join(", ", command.Data.Options.First().Value);
            if (!_app.GetJoinOrderHandler().JoinOrder(serverID, groupbuyingID, user))
            {
                await command.RespondAsync("加入團購失敗", ephemeral: true);
                return;
            }
            GroupBuying groupBuying = new GroupBuying(new GroupBuyingsBoundary(), groupbuyingID);
            string storeID = groupBuying.GetStore().StoreID;
            List<StoreItem> storeItems = _app.GetJoinOrderHandler().ListItemsOfStore(storeID, serverID, new StoresBoundary());
            SelectMenuBuilder operatorBuilder = BuildOperatorTypeMenu();
            SelectMenuBuilder storeitemBuilder = BuildStoreItemMenu(storeItems);
            SelectMenuBuilder itemQuantityBuilder = BuildStoreItemQuantityMenu(10);
            ButtonBuilder editItemButtonBuilder = BuildEditOrderButton();
            ButtonBuilder subnitOrderButtonBuilder = BuildSubmitOrderButton();
            ComponentBuilder messageBuilder = new ComponentBuilder()
                .WithSelectMenu(operatorBuilder)
                .WithSelectMenu(storeitemBuilder)
                .WithSelectMenu(itemQuantityBuilder)
                .WithButton(editItemButtonBuilder)
                .WithButton(subnitOrderButtonBuilder);
            await command.RespondAsync(components : messageBuilder.Build(), ephemeral: true);
        }

        private SelectMenuBuilder BuildOperatorTypeMenu()
        {
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("請選擇操作模式")
                .WithCustomId(OPERATOR_TYPE_ID);
            menuBuilder.AddOption($"新增商品", $"0");
            menuBuilder.AddOption($"編輯商品", $"1");
            menuBuilder.AddOption($"刪除商品", $"2");
            return menuBuilder;
        }

        private async Task OperatorMenuHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != OPERATOR_TYPE_ID)
                return;
            string[] value = string.Join(", ", messageComponent.Data.Values).Split('-');
            string operatorID = value[0];
            string userID = messageComponent.User.Id.ToString();
            if (!_joinOrderUIData.ContainsKey(userID))
            {
                _joinOrderUIData.Add(userID, new JoinOrderUIData());
            }
            _joinOrderUIData[userID].OPERATOR = operatorID;
            await messageComponent.DeferAsync();
        }

        private SelectMenuBuilder BuildStoreItemMenu(List<StoreItem> storeItems)
        {
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("請選擇商品")
                .WithCustomId(STORE_ITEM_ID);
            foreach (StoreItem storeitem in storeItems)
            {
                menuBuilder.AddOption($"{storeitem.StoreitemName}({storeitem.StoreitemPrice}元)", $"{storeitem.StoreitemID}");
            }
            return menuBuilder;
        }

        private async Task StoreItemMenuHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != STORE_ITEM_ID)
                return;
            string[] value = string.Join(", ", messageComponent.Data.Values).Split('-');
            string storeitemID = value[0];
            string userID = messageComponent.User.Id.ToString();
            if (!_joinOrderUIData.ContainsKey(userID))
            {
                _joinOrderUIData.Add(userID, new JoinOrderUIData());
            }
            _joinOrderUIData[userID].ITEMID = storeitemID;
            await messageComponent.DeferAsync();
        }

        private SelectMenuBuilder BuildStoreItemQuantityMenu(int maxQuantity)
        {
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("請選擇數量")
                .WithCustomId(STORE_ITEM_QUANTITY_ID);
            for(int i = 1;i < maxQuantity+1;i++)
            {
                menuBuilder.AddOption($"{i}", $"{i}");
            }
            return menuBuilder;
        }

        private async Task StoreQuantityMenuHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != STORE_ITEM_QUANTITY_ID)
                return;
            string[] value = string.Join(", ", messageComponent.Data.Values).Split('-');
            string quantityID = value[0];
            string userID = messageComponent.User.Id.ToString();
            if (!_joinOrderUIData.ContainsKey(userID))
            {
                _joinOrderUIData.Add(userID, new JoinOrderUIData());
            }
            _joinOrderUIData[userID].QUANTITY = Int32.Parse(quantityID);
            await messageComponent.DeferAsync();
        }

        private ButtonBuilder BuildEditOrderButton()
        {
            var buttonBuilder = new ButtonBuilder()
                .WithLabel("點此送出更動")
                .WithCustomId(EDIT_ITEM_BTN_ID)
                .WithStyle(ButtonStyle.Primary);
            return buttonBuilder;
        }

        private async Task EditOrderButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != EDIT_ITEM_BTN_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            if (!_joinOrderUIData.ContainsKey(userID))
            {
                await messageComponent.RespondAsync("尚未選擇操作模式", ephemeral: true);
                return;
            }
            if (_joinOrderUIData[userID].OPERATOR == "-1")
            {
                await messageComponent.RespondAsync("尚未選擇操作模式", ephemeral: true);
                return;
            }
            if (_joinOrderUIData[userID].ITEMID == "-1")
            {
                await messageComponent.RespondAsync("尚未選擇商品", ephemeral: true);
                return;
            }
            if (_joinOrderUIData[userID].QUANTITY == 0)
            {
                await messageComponent.RespondAsync("尚未選擇購買數量", ephemeral: true);
                return;
            }
            if (_joinOrderUIData[userID].OPERATOR == "0")
            {
                _app.GetJoinOrderHandler().AddItem(user, _joinOrderUIData[userID].ITEMID, _joinOrderUIData[userID].QUANTITY);
            }
            else if (_joinOrderUIData[userID].OPERATOR == "1")
            {
                _app.GetJoinOrderHandler().EditItem(user, _joinOrderUIData[userID].ITEMID, _joinOrderUIData[userID].QUANTITY);
            }
            else if (_joinOrderUIData[userID].OPERATOR == "2")
            {
                _app.GetJoinOrderHandler().DeleteItem(user, _joinOrderUIData[userID].ITEMID);
            }
            await messageComponent.RespondAsync("已更新訂單列表", ephemeral: true);
        }

        private ButtonBuilder BuildSubmitOrderButton()
        {
            var buttonBuilder = new ButtonBuilder()
                .WithLabel("結束更動以及送出訂單")
                .WithCustomId(SUBMIT_ORDER_BTN_ID)
                .WithStyle(ButtonStyle.Primary);
            return buttonBuilder;
        }

        private async Task SubmitOrderButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != SUBMIT_ORDER_BTN_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            if (!_joinOrderUIData.ContainsKey(userID))
            {
                await messageComponent.RespondAsync("尚未選擇操作模式", ephemeral: true);
                return;
            }
            await messageComponent.RespondAsync($"已完成團購點餐\n總共花費:{_app.GetJoinOrderHandler().GetTotal(user)}元", ephemeral: true);
            _app.GetJoinOrderHandler().SubmitOrder(user);
        }
    }
}
