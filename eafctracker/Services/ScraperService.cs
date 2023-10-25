using System.Net;
using Eafctracker.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;


namespace Eafctracker.Services
{
    public class ScraperService : IScraperService
    {
        
        private readonly IHttpClientService _httpClientService;
        private const string LINK = "player/";
        private const string LATEST_PLAYERS_LINK = "https://www.futbin.com/latest";

        public ScraperService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        HtmlDocument? PlayerPage { get; set; }
        
        public async Task<Card?> GetCard(int id)
        {
            HtmlDocument? page = await GetHtmlDocument(LINK+id.ToString());
            if (page is null)
                return null;
            
            var name = ParseFromDoc(page, "(//td[@class='table-row-text'])[1]");
            var FbDataId = ParseFromDoc(page, "//th[text()='ID']/following-sibling::td");
            var price = await GetPriceAsync(int.Parse(FbDataId));
            var psPrices = GetPsPrices(price);
            var pcPrices = GetPcPrices(price);
            var revision = ParseFromDoc(page, "//th[text()='Revision']/following-sibling::td");
            var raiting = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[2]");
            var position = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[4]");
            var displayedName = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[3]");
            
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

        public async Task Getpage(Uri uri)
        {
            throw new NotImplementedException();
        }

        public async Task GetCardsIdList(Uri uri)
        {
            Task<HtmlDocument?> page = GetHtmlDocument(uri.ToString());
            var pagesCount = int.Parse(ParseFromDoc(await page, "(//a[contains(@class,'page-link  waves-effect waves-effect')])[6]"));
            for (int i = 1; i <= pagesCount; i++)
            {
                var pageUri = new Uri(uri, $"?page={i}&ps_price=20000-15000000");
                
            }

        }

        public async Task<int> GetMaxId()
        {
            var doc = await GetHtmlDocument(LATEST_PLAYERS_LINK);
            if (doc is null)
                return 0;

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class=' get-tp']");
            string dataSiteId = node?.Attributes["data-site-id"]?.Value;

            if (dataSiteId != null)
                return int.Parse(dataSiteId);
            
            else
                return 0;
        }
        
   
        private Ps GetPsPrices(string priceResponse)
            =>  JsonConvert.DeserializeObject<Ps>(priceResponse);

        private Pc GetPcPrices(string priceResponse)
            =>  JsonConvert.DeserializeObject<Pc>(priceResponse);

        private async Task<string> GetPriceAsync(int FbDataId)
        {
            var client = _httpClientService.GetHttpClient();
            return await client.GetStringAsync($"playerPrices?player={FbDataId}");
        }
        
        private async Task<HtmlDocument?> GetHtmlDocument(string link)
        {
            var client = _httpClientService.GetHttpClient();
            var response = await client.GetStringAsync(link);
            
            var doc = new HtmlDocument();
            doc.LoadHtml(response);
            return doc;
        }   

        public string ParseFromDoc(HtmlDocument page,string xPath)
            =>  page.DocumentNode
                .SelectSingleNode(xPath)
                .InnerText;
    }
}