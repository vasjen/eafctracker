using Eafctracker.Data;
using Eafctracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eafctracker.Services {

    public static class SeedData {

        public static async Task EnsurePopulatedAsync(IHost host) {
            ApplicationDbContext context = host.Services
                .CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var initial = host.Services.GetRequiredService<IInitialService>();
            
            if (context.Database.GetPendingMigrations().Any()) {
                context.Database.Migrate();
            }
          
           
            if (!context.Cards.Any()) {
           
                var cards = await initial.GetCardsRangeAsync();
                await context.AddRangeAsync(cards);
                await context.SaveChangesAsync();
                System.Console.WriteLine("List of cards was added");
            }


        }
    }
}
