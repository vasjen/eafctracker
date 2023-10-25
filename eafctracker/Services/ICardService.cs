

using Eafctracker.Models;

namespace Cards.Services
{
    public interface ICardService
    {
            Task<Card> CreateCard(Card card);
            Task<Card?> GetCard(int id);
            Task UpdateCard(CardUpdateRequest card);
            Task DeleteCard(int id);
            Task<int> GetMaxId();
    }
}
