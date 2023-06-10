using Discord;
using Discord.Net;
using Discord.WebSocket;
using groupOrdering.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.UI
{
    public class EndOrderUI
    {
        private readonly DiscordSocketClient _client;
        private readonly GroupBuyingApp _app;
        private readonly EndGroupBuyingHandler _handler;

        private const string END_ORDER_COMMAND = "end-order";

        public EndOrderUI(DiscordSocketClient client, GroupBuyingApp app)
        {
            _client = client;
            _app = app;
            _handler = _app.GetEndGroupBuyingHandler();

            _client.Ready += EndOrder_Ready;
            _client.SlashCommandExecuted += EndOrderCommandHandler;
        }

        private async Task EndOrder_Ready()
        {
            const ulong TEST_GUILD_ID = 1093073444440133684;

            var guild = _client.GetGuild(TEST_GUILD_ID);
            var createOrderCommand = new SlashCommandBuilder()
                .WithName(END_ORDER_COMMAND)
                .WithDescription("結束團購")
                .AddOption("團購編號", ApplicationCommandOptionType.String, "請輸入欲結束的團購編號", isRequired: true); ;
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

        private async Task EndOrderCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name != END_ORDER_COMMAND)
                return;

            string userID = command.User.Id.ToString();
            User user = new User(userID);
            string groupbuyingID = string.Join(", ", command.Data.Options.First().Value);
            await command.RespondAsync(text: _handler.EndGroupBuying(user, groupbuyingID), ephemeral: true);
        }
    }
}
