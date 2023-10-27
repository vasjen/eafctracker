using Eafctracker.Models;

namespace Eafctracker.Services.Interfaces
{
    public interface IScraperService
    {
        Task<Card?> GetCard(int id);

    }
}