using Eafctracker.Models;
using Eafctracker.Services.Interfaces;
using HtmlAgilityPack;
using Newtonsoft.Json;


namespace Eafctracker.Services
{
    public class ScraperService : IScraperService
    {
        
        // private readonly IHttpClientService _httpClientService;
        private const string link = "player/";
        private const string latestPlayersLink = "https://www.futbin.com/latest";
       
        
        
        public async Task<Card?> GetCard(int id)
        {
            HtmlDocument? page = await GetHtmlDocument(link+id.ToString());
            if (page is null)
                return null;
            
            string name = ParseFromDoc(page, "(//td[@class='table-row-text'])[1]");
            string fbDataId = ParseFromDoc(page, "//th[text()='ID']/following-sibling::td");
            string price = await GetPriceAsync(Int32.Parse(fbDataId));
            Ps psPrices = GetPsPrices(price);
            Pc pcPrices = GetPcPrices(price);
            string revision = ParseFromDoc(page, "//th[text()='Revision']/following-sibling::td");
            string rating = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[2]");
            string position = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[4]");
            string displayedName = ParseFromDoc(page, "//*[@id=\"Player-card\"]/div[3]");
            
            return new Card
            {
                Id = id,
                FbId = id,
                FbDataId = Int32.Parse(fbDataId),
                Name = name,
                DisplayedName = displayedName,
                PcPrices = pcPrices,
                PsPrices = psPrices,
                Position = position,
                Revision = revision,
                Rating = Int32.Parse(rating)
            };
        }

        public async Task<int> GetMaxId()
        {
            HtmlDocument? doc = await GetHtmlDocument(latestPlayersLink);
            if (doc is null)
                return 0;

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class=' get-tp']");
            string dataSiteId = node?.Attributes["data-site-id"]?.Value;

            return dataSiteId != null ? Int32.Parse(dataSiteId) : 0;
        }
        
        private async Task<HtmlDocument?> GetHtmlDocument(string link)
        {   
            var req = await _httpClientService.MakeRequestUsingRandomProxy(link);
            if (req is null)
                return null;
            
            var page = await req.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(page);
            return doc;
        }
        private Ps GetPsPrices(string priceResponse)
            =>  JsonConvert.DeserializeObject<Ps>(priceResponse);

        private Pc GetPcPrices(string priceResponse)
            =>  JsonConvert.DeserializeObject<Pc>(priceResponse);

        private async Task<string> GetPriceAsync(int FbDataId)
        {
            var request = await _httpClientService.MakeRequestUsingRandomProxy($"24/playerPrices?player={FbDataId}");
            return await request.Content.ReadAsStringAsync();
        
        }
        public async Task<IEnumerable<SalesHistory>?> GetSalesHistoryAsync (int fbDataId) {
            string URL="https://www.futbin.com/23/getPlayerSales?platform=ps&resourceId=";
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,URL+fbDataId);
            var _client = _service.GetHttpClient();
            var httpResponseMessage = await _client.SendAsync(httpRequestMessage);
            var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        
            try {
                Histories = await JsonSerializer.DeserializeAsync<IEnumerable<SalesHistory>>(contentStream);
           
            }
            catch (JsonException ex)
            {
                System.Console.WriteLine(ex.Message);
                var code = httpResponseMessage.IsSuccessStatusCode;
                System.Console.WriteLine(code);
                System.Console.WriteLine("The item doesn't contains a sales history");
            }
            return Histories;
           
        }
        
            

        public string ParseFromDoc(HtmlDocument page,string xPath)
            =>  page.DocumentNode
                .SelectSingleNode(xPath)
                .InnerText;
    }
}