using System.Collections;
using Eafctracker.Data;
using Eafctracker.Models;
using Eafctracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
namespace Eafctracker.Services {
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramBotClient _client;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public readonly string _chatId;
        private const string URL="https://www.futbin.com/23/player/";

        public TelegramService(ITelegramBotClient client, ApplicationDbContext context, IConfiguration config)
    {
        _client=client;
        _context=context;
        _config=config;
        _chatId=config.GetSection("Telegram").GetValue<string>("ChatId");
    }

       
        public async Task SendInfo(Profit ProfitPlayer, int avgPrice, IEnumerable<SalesHistory> lastTenSales)
        {
            var Notification = await CreateNotificationAsync(ProfitPlayer, avgPrice, lastTenSales);
            await _client.SendTextMessageAsync(_chatId, Notification+$"\n &#11015 Add your deals in comments &#11015", (int?)ParseMode.Html ,disableWebPagePreview: true,  allowSendingWithoutReply: true );
                        
        }

         async Task<string> CreateNotificationAsync(Profit ProfitPlayer, int avgPrice, IEnumerable<SalesHistory> lastTenSales)
         {

             return "";
         }


        
    }
}