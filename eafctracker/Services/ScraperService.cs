using System.Net;
using Eafctracker.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;


namespace Eafctracker.Services
{
    public class ScraperService : IScraperService
    {
        
        // private readonly IHttpClientService _httpClientService;
        private const string link = "24/player/";
        private const string latestPlayersLink = "https://www.futbin.com/latest";

        public ScraperService()
        {
           
        }
        HtmlDocument? PlayerPage { get; set; }
        
        public async Task<Card?> GetCard(int id)
        {
            var page = await GetHtmlDocument(link+id.ToString());
            if (page is null)
                return null;
            
            var name = ParseFromDoc(page, "(//td[@class='table-row-text'])[1]");
            System.Console.WriteLine("Name: {0}", name);
            var FbDataId = ParseFromDoc(page, "//th[text()='ID']/following-sibling::td");
            System.Console.WriteLine("FbDataId: {0}", FbDataId);
            var price = await GetPriceAsync(int.Parse(FbDataId));
            System.Console.WriteLine("Price: {0}", price);
            var psPrices = GetPsPrices(price);
            System.Console.WriteLine("PsPrices: {0}", psPrices.LCPrice);
            var pcPrices = GetPcPrices(price);
            System.Console.WriteLine("PcPrices: {0}", pcPrices.LCPrice);
            var revision = ParseFromDoc(page, "//th[text()='Revision']/following-sibling::td");
            System.Console.WriteLine("Revision: {0}", revision);
            var raiting = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[2]");
            System.Console.WriteLine("Raiting: {0}", raiting);
            var position = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[4]");
            System.Console.WriteLine("Position: {0}", position);
            var displayedName = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[3]");
            System.Console.WriteLine("DisplayedName: {0}", displayedName);
            
            return new Card
            {
                Id = id,
                FbId = id,
                FbDataId = int.Parse(FbDataId),
                Name = name,
                DisplayedName = displayedName,
                PcPrices = pcPrices,
                PsPrices = psPrices,
                Position = position,
                Revision = revision,
                Raiting = int.Parse(raiting)
            };
        }

        public async Task<int> GetMaxId()
        {
            var doc = await GetHtmlDocument(latestPlayersLink);
            if (doc is null)
                return 0;

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class=' get-tp']");
            string dataSiteId = node?.Attributes["data-site-id"]?.Value;

            if (dataSiteId != null)
                return int.Parse(dataSiteId);
            
            else
                return 0;
        }
        
        private async Task<HtmlDocument?> GetHtmlDocument(string link)
        {   
            // var req = await _httpClientService.MakeRequestUsingRandomProxy(link);
            // if (req is null)
                // return null;
            
            // var page = await req.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            // doc.LoadHtml(page);
            return doc;
        }
        private Ps GetPsPrices(string priceResponse)
            =>  JsonConvert.DeserializeObject<Ps>(priceResponse);

        private Pc GetPcPrices(string priceResponse)
            =>  JsonConvert.DeserializeObject<Pc>(priceResponse);

        private async Task<string> GetPriceAsync(int FbDataId)
        {
            // var request = await _httpClientService.MakeRequestUsingRandomProxy($"24/playerPrices?player={FbDataId}");
            // return await request.Content.ReadAsStringAsync();
            return "";
        }
        
            

        public string ParseFromDoc(HtmlDocument page,string xPath)
            =>  page.DocumentNode
                .SelectSingleNode(xPath)
                .InnerText;
    }
}