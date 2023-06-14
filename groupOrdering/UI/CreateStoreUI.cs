using Discord.Net;
using Discord.WebSocket;
using Discord;
using groupOrdering.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.UI
{
    public class CreateStoreUI
    {
        private readonly DiscordSocketClient _client;
        private readonly CreateStoreHandler _handler;

        private const string CREATE_STORE_COMMAND = "create-store";
        private const string ADD_ITEM_BUTTON_ID = "add-item-button";
        private const string DELETE_ITEM_BUTTON_ID = "delete-item-button";
        private const string REFRESH_ITEM_BUTTON_ID = "refresh-item-button";
        private const string END_EDIT_STORE_BUTTON_ID = "end-edit-store-button";
        private const string ADD_ITEM_MODAL_ID = "add-item-modal";
        private const string DELETE_ITEM_MODAL_ID = "delete-item-modal";
        private const string ITEM_NAME_TEXT_ID = "item-name-text-input";
        private const string ITEM_PRICE_TEXT_ID = "item-price-text-input";

        public CreateStoreUI(DiscordSocketClient client, GroupBuyingApp app)
        {
            _client = client;

            _handler = app.GetCreateStoreHandler();

            _client.Ready += CreateStore_Ready;
            _client.SlashCommandExecuted += CreateStoreCommandHandler;
            _client.ButtonExecuted += AddItemButtonHandler;
            _client.ButtonExecuted += EndEditStoreButtonHandler;
            _client.ButtonExecuted += DeleteItemButtonHandler;
            _client.ButtonExecuted += RefreshButtonHandler;
            _client.ModalSubmitted += AddItemModalHandler;
            _client.ModalSubmitted += DeleteItemModalHandler;
        }

        private async Task CreateStore_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(CREATE_STORE_COMMAND)
                .WithDescription("創建一家商店")
                .AddOption("商店名稱", ApplicationCommandOptionType.String, "請輸入欲創建的商店名稱", isRequired: true)
                .AddOption("商店地點", ApplicationCommandOptionType.String, "請輸入商店所在城市與街道", isRequired: true)
                .AddOption("商店電話號碼", ApplicationCommandOptionType.String, "請輸入商店的電話號碼", isRequired: true);
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
        private Embed BuildStoreDetail(Store store)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<StoreItem> storeItems = store.GetStoreItems();
            foreach (StoreItem storeItem in storeItems)
            {
                stringBuilder.AppendLine($"{storeItem.StoreitemName} {storeItem.StoreitemPrice}元");
            }
            EmbedBuilder embedBuilder = new EmbedBuilder()
                .WithTitle(store.StoreName)
                .WithFooter($"商品數量: {store.GetStoreItemCount()}")
                .WithDescription(stringBuilder.ToString());
            return embedBuilder.Build();
        }

        private async Task CreateStoreCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != CREATE_STORE_COMMAND)
                return;
            string userID = command.User.Id.ToString();
            User user = new User(userID);
            string serverID = command.GuildId.ToString() ?? "";

            string storeName = string.Join(", ", command.Data.Options.First(x => x.Name == "商店名稱").Value);
            string storeAddress = string.Join(", ", command.Data.Options.First(x => x.Name == "商店地點").Value);
            string phoneNumber = string.Join(", ", command.Data.Options.First(x => x.Name == "商店電話號碼").Value);
            _handler.CreateStore(user, serverID, storeName, storeAddress, phoneNumber);

            ComponentBuilder storeItemBrowser = BuildStoreItemBrowser();
            Store store = _handler.GetStore(user);
            Embed storeDetail = BuildStoreDetail(store);
            await command.RespondAsync(embed: storeDetail, components: storeItemBrowser.Build(), ephemeral: true);
        }

        private ComponentBuilder BuildStoreItemBrowser()
        {
            var deleteItemButtonBuilder = new ButtonBuilder()
                    .WithLabel("刪除商品")
                    .WithCustomId(DELETE_ITEM_BUTTON_ID)
                    .WithStyle(ButtonStyle.Danger);
            var refreshButton = new ButtonBuilder()
                .WithLabel("重整")
                .WithCustomId(REFRESH_ITEM_BUTTON_ID)
                .WithStyle(ButtonStyle.Secondary);
            var addItemButtonBuilder = new ButtonBuilder()
                .WithLabel("新增商品")
                .WithCustomId(ADD_ITEM_BUTTON_ID)
                .WithStyle(ButtonStyle.Primary);
            var endBuildStoreButtonBuilder = new ButtonBuilder()
                    .WithLabel("點此結束編輯店家")
                    .WithCustomId(END_EDIT_STORE_BUTTON_ID)
                    .WithStyle(ButtonStyle.Primary);
            var builder = new ComponentBuilder();
            builder.AddRow(new ActionRowBuilder().WithButton(deleteItemButtonBuilder).WithButton(refreshButton).WithButton(addItemButtonBuilder))
                .AddRow(new ActionRowBuilder().WithButton(endBuildStoreButtonBuilder));
            return builder;
        }

        private async Task AddItemButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != ADD_ITEM_BUTTON_ID)
                return;
            var modalBuilder = new ModalBuilder()
                .WithTitle("加入商品")
                .WithCustomId(ADD_ITEM_MODAL_ID)
                .AddTextInput("商品名稱", ITEM_NAME_TEXT_ID, TextInputStyle.Short, "請輸入商品名稱", required: true, maxLength: 50)
                .AddTextInput("商品價格", ITEM_PRICE_TEXT_ID, TextInputStyle.Short, "請輸入商品價格", required: true, maxLength: 10);
            Modal modal = modalBuilder.Build();
            await messageComponent.RespondWithModalAsync(modal);
        }

        private async Task DeleteItemButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != DELETE_ITEM_BUTTON_ID)
                return;
            var modalBuilder = new ModalBuilder()
                .WithTitle("刪除商品")
                .WithCustomId(DELETE_ITEM_MODAL_ID)
                .AddTextInput("商品名稱", ITEM_NAME_TEXT_ID, TextInputStyle.Short, "請輸入欲刪除的商品名稱", required: true, maxLength: 50);
            Modal modal = modalBuilder.Build();
            await messageComponent.RespondWithModalAsync(modal);
        }

        private async Task RefreshButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != REFRESH_ITEM_BUTTON_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            Store store = _handler.GetStore(user);
            Embed storeDetail = BuildStoreDetail(store);
            await messageComponent.UpdateAsync(x => x.Embed = storeDetail);
        }

        private async Task EndEditStoreButtonHandler(SocketMessageComponent messageComponent)
        {
            if (messageComponent.Data.CustomId != END_EDIT_STORE_BUTTON_ID)
                return;
            string userID = messageComponent.User.Id.ToString();
            User user = new User(userID);
            _handler.EndBuildStore(user);
            await messageComponent.UpdateAsync(x => { x.Content = "已成功建立店家"; x.Components = null; x.Embed = null; });
        }

        private async Task AddItemModalHandler(SocketModal modal)
        {
            if (modal.Data.CustomId != ADD_ITEM_MODAL_ID)
                return;
            string userID = modal.User.Id.ToString();
            User user = new User(userID);
            try
            {
                string itemName = modal.Data.Components.First(x => x.CustomId == ITEM_NAME_TEXT_ID).Value;
                int price = int.Parse(modal.Data.Components.First(x => x.CustomId == ITEM_PRICE_TEXT_ID).Value);
                _handler.AddStoreItem(user, itemName, price);
                await modal.DeferAsync();
            }
            catch (FormatException)
            {
                await modal.RespondAsync("請在價錢欄位輸入數字", ephemeral: true);
            }
            catch (Exception)
            {
                await modal.RespondAsync("發生錯誤", ephemeral: true);
            }
        }

        private async Task DeleteItemModalHandler(SocketModal modal)
        {
            if (modal.Data.CustomId != DELETE_ITEM_MODAL_ID)
                return;
            string userID = modal.User.Id.ToString();
            User user = new User(userID);
            string itemName = modal.Data.Components.First(x => x.CustomId == ITEM_NAME_TEXT_ID).Value;
            _handler.DeleteStoreItem(user, itemName);
            await modal.DeferAsync();
        }
    }
}
