using Eafctracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eafctracker.Services
{
    public interface IScraperService
    {
        Task<Card?> GetCard(int id);
        Task Getpage(Uri uri);
    }
}