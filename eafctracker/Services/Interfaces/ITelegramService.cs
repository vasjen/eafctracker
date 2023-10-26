using Eafctracker.Models;

namespace Eafctracker.Services.Interfaces
{
    public interface ITelegramService
    {
         Task SendInfo (Profit ProfitPlayer, int avgPrice, IEnumerable<SalesHistory> lastTenSales);
 
    }
}