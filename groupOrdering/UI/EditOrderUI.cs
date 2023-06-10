using Discord.Net;
using Discord;
using Discord.WebSocket;
using groupOrdering.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupOrdering.Boundary;

namespace groupOrdering.UI
{
    public class EditOrderUI
    {
        private readonly DiscordSocketClient _client;
        private readonly GroupBuyingApp _app;
        private EditOrderHandler _handler;
        private readonly Dictionary<string, JoinOrderUIData> _joinOrderUIData;
        
        private const string SELECT_ORDER_COMMAND = "select-order";
        private const string OPERATOR_TYPE_ID = "edit-operator-type";
        private const string STORE_ITEM_ID = "edit-store-item";
        private const string STORE_ITEM_QUANTITY_ID = "edit-store-item-quantity";
        private const string EDIT_ITEM_BTN_ID = "edit-edit-item-btn";
        private const string END_EDIT_ORDER_BTN_ID = "end-edit-order-btn";

        public EditOrderUI(DiscordSocketClient client, GroupBuyingApp app)
        {
            _client = client;
            _app = app;
            _handler = _app.GetEditOrderHandler();
            _joinOrderUIData = new Dictionary<string, JoinOrderUIData>();

            _client.Ready += EditOrder_Ready;
            _client.SlashCommandExecuted += SelectOrderCommandHandler;
            _client.SelectMenuExecuted += OperatorMenuHandler;
            _client.SelectMenuExecuted += StoreItemMenuHandler;
            _client.SelectMenuExecuted += StoreQuantityMenuHandler;
            _client.ButtonExecuted += EditOrderButtonHandler;
            _client.ButtonExecuted += EndEditOrderButtonHandler;
        }

        private async Task EditOrder_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(SELECT_ORDER_COMMAND)
                .WithDescription("修改團購訂單")
                .AddOption("團購編號", ApplicationCommandOptionType.String, "請輸入欲編輯的團購編號", isRequired: true);
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

        private async Task SelectOrderCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != SELECT_ORDER_COMMAND)
                return;

            string userID = command.User.Id.ToString();
            User user = new User(userID);
            string serverID = command.GuildId.ToString() ?? "";
            string groupbuyingID = string.Join(", ", command.Data.Options.First().Value);

            GroupBuying groupBuying = new GroupBuying(new GroupBuyingsBoundary(), groupbuyingID);
            string storeID = groupBuying.GetStore().StoreID;
            List<StoreItem> storeItems = _app.GetJoinOrderHandler().ListItemsOfStore(storeID, serverID, new StoresBoundary());
            _handler.SelectGroupBuying(user, groupbuyingID);
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
            await command.RespondAsync(components: messageBuilder.Build(), ephemeral: true);
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

        private SelectMenuBuilder BuildStoreItemQuantityMenu(int maxQuantity)
        {
            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("請選擇數量")
                .WithCustomId(STORE_ITEM_QUANTITY_ID);
            for (int i = 1; i < maxQuantity + 1; i++)
            {
                menuBuilder.AddOption($"{i}", $"{i}");
            }
            return menuBuilder;
        }

        private ButtonBuilder BuildEditOrderButton()
        {
            var buttonBuilder = new ButtonBuilder()
                .WithLabel("點此送出更動")
                .WithCustomId(EDIT_ITEM_BTN_ID)
                .WithStyle(ButtonStyle.Primary);
            return buttonBuilder;
        }

        private ButtonBuilder BuildSubmitOrderButton()
        {
            var buttonBuilder = new ButtonBuilder()
                .WithLabel("結束更動以及送出訂單")
                .WithCustomId(END_EDIT_ORDER_BTN_ID)
                .WithStyle(ButtonStyle.Primary);
            return buttonBuilder;
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
                _handler.AddItem(user, _joinOrderUIData[userID].ITEMID, _joinOrderUIData[userID].QUANTITY);
            }
            else if (_joinOrderUIData[userID].OPERATOR == "1")
            {
                _handler.EditItem(user, _joinOrderUIData[userID].ITEMID, _joinOrderUIData[userID].QUANTITY);
            }
            else if (_joinOrderUIData[userID].OPERATOR == "2")
            {
                _handler.DeleteItem(user, _joinOrderUIData[userID].ITEMID);
            }
            await messageComponent.RespondAsync("已更新訂單列表", ephemeral: true);
        }

        private async Task EndEditOrderButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != END_EDIT_ORDER_BTN_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            if (!_joinOrderUIData.ContainsKey(userID))
            {
                await messageComponent.RespondAsync("尚未選擇操作模式", ephemeral: true);
                return;
            }
            await messageComponent.RespondAsync($"已完成修改團購點餐\n總共花費:{_handler.GetTotal(user)}元", ephemeral: true);
            _handler.EndEdit(user);
        }
    }
}
