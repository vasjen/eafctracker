using Eafctracker.Models;

namespace Eafctracker.Services.Interfaces
{
    public interface ISalesHistoryService
    {
       public Task<IEnumerable<SalesHistory>?> GetSalesHistoryAsync (int fbDataId);
    }
}