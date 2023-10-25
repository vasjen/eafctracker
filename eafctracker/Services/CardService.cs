using Eafctracker.Data;
using Eafctracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards.Services
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _context;

        public CardService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Card> CreateCard(Card card)
        {
            // var card = new Card
            // {
                // Name = cardCreateRequest.Name,
                // Revision = cardCreateRequest.Description
            // };
            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
            return card;
            
        }

        public async Task DeleteCard(int id)
        {
            var card = await _context.FindAsync<Card>(id);
            if (card is not null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Card?> GetCard(int id)
            => await _context.FindAsync<Card>(id);

        public async Task UpdateCard(CardUpdateRequest cardUpdateRequest)
        {
            var card = await _context.FindAsync<Card>(cardUpdateRequest.Id);
            if (card is not null)
            {
                card.Name = cardUpdateRequest.Name;
                card.Revision = cardUpdateRequest.Description;
                _context.Cards.Update(card);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetMaxId()
        {
            var card = await _context.Cards.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return card is null ? 0 : card.Id;
        }
    }
}
