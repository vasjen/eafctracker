using Eafctracker.Models;

namespace Eafctracker.Services.Interfaces
{
    public interface IInitialService
    {   
            Task<IEnumerable<Card>> GetCardsRangeAsync();
            Task<Card> GetNewCardAsync(int FbId);
            

         
    }
}