using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using Eafctracker.Models;
using Eafctracker.Services.Interfaces;

namespace Eafctracker.Services {

    public  class InitialService (IScraperService scraperService) : IInitialService
    {
        private readonly IScraperService _scraperService = scraperService;
     
        private const string URL_PLAYER = "https://www.futbin.com/player/";
        private const string URL_PLAYERS = URL_PLAYER + "s";
            // "https://www.futbin.com/23/players?page=1&player_rating=82-99&ps_price=15000-15000000"
        public  async Task<IEnumerable<Card>> GetCardsRangeAsync(){
            

            List<Card> cards = new List<Card>();
            string stringId=string.Empty;
            string name=string.Empty;
            string raiting=string.Empty;
            string version=string.Empty;
            string position=string.Empty;
            int counter=0;
            int MaxPage=await GetMaxNumberPage("https://www.futbin.com/23/players?page=1&player_rating=82-99&ps_price=15000-15000000");
            for (int pageNumber=1; pageNumber<=MaxPage; pageNumber++)
             {  
                try 
                {
                    var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get, $"https://www.futbin.com/23/players?page={pageNumber}&player_rating=82-99&ps_price=15000-15000000");
                    var _client = _service.GetHttpClient();
                    
                    var httpResponseMessage = await _client.SendAsync(httpRequestMessage);
                    var response = await httpResponseMessage.Content.ReadAsStringAsync();
                  
                    await Task.Delay(1000);
                    string[] result = response.Split(Environment.NewLine);
                    string findingString =string.Empty;
                    
                    for (int i=0;i<result.Length;i++)
                    {   //Getting FbId
                        if (result[i].Contains("data-site-id=") && !(result[i].Contains("<") || result[i].Contains("}") ))


                        {   stringId=result[i].Remove(result[i].LastIndexOf('"')).Substring(result[i].IndexOf('"')+1);
                           
                        }
                       
                        //Getting Name
                        if (result[i].Contains("data-tp-type=\"player\""))
                        {
                            name=result[i+1].Remove(result[i+1].LastIndexOf('<')).Substring(result[i+1].IndexOf('>')+1);
                        }
                        //Getting Raiting
                        if (result[i].Contains("form rating ut23") && !result[i].Contains("lazy"))
                        {
                            raiting = result[i].Substring(result[i].IndexOf("</span")-2,2);
                        }
                        //Getting Position
                        if (result[i].Contains("<div class=\"font-weight-bold\">")) 
                        {
                            position=result[i].Remove(result[i].LastIndexOf("<")).Substring(result[i].IndexOf(">")+1);
                        }
                        //Getting Version
                        if (result[i].Contains("<td class=\"mobile-hide-table-col\">") ) {

                            version=result[i+1].Remove(result[i+1].LastIndexOf("<")).Substring(result[i+1].IndexOf(">")+1);
                        }
                        //Getting Price
                        if (result[i].Contains("<span class=\" font-weight-bold\">"))
                        {   counter++;
                            string priceS=result[i].Remove(result[i].IndexOf("<img")-1).Substring(result[i].IndexOf("\">")+2);
                            cards.Add(new Card {FbId=int.Parse(stringId), Name= name, Revision= version,Rating=int.Parse(raiting),Position=position});
                            System.Console.WriteLine($"[{counter}] Added {name} + {version}");
                        }
                        
                    }
                }    
             catch (Exception ex)
             {
                System.Console.WriteLine(ex.Message);
             }
             System.Console.WriteLine($"Added from {pageNumber} page of total: {MaxPage}");
            }
            System.Console.WriteLine($"Total added a {counter} cards");
            Parallel.ForEach(cards, new ParallelOptions {MaxDegreeOfParallelism = 5}, async p=> { 
                
                    GetDataId(p);
                    IsTradeble(p).Wait();

            });

        return cards;
        
        }

        public async Task<Card?> GetNewCardAsync(int FbId)
            => await _scraperService.GetCard(FbId);
       

        private async Task<int> GetMaxNumberPage(string Url)
        {
            var client = _service.GetHttpClient();
            var result = await Scraping.GetPageAsStrings(client,Url);
            string NumberString=string.Empty;
            int MaxPage=0;
            for (int i=result.Length-1;i>0;i--)
            {
                if (result[i].Contains("page-link \">"))
                {
                     NumberString = result[i];
                     MaxPage=int.Parse(NumberString.Remove(NumberString.IndexOf("</a")).Substring(NumberString.IndexOf("\">")+2));
                     break;
                    
                }
            }
            System.Console.WriteLine("Max page is {0}", MaxPage);
            return MaxPage;
        }
        private async  Task IsTradeble(Card card)
         {
          
            string Updated=string.Empty;
            var _client = _service.GetHttpClient();

            string requestUri = $"http://futbin.com/23/playerPrices?player={card.FbDataId}";
               
                
            Task.Delay(1500).Wait();
            var response =  await _client.GetAsync(requestUri);
             string PriceLowest=string.Empty;
             string PriceNext=string.Empty;
             string jsonResponse = string.Empty;
            
                    try 
                    {
                        if (response.IsSuccessStatusCode)   
                        jsonResponse = await response.Content.ReadAsStringAsync();
                        JsonNode jsonNod = JsonNode.Parse(jsonResponse);
                        Updated = jsonNod[$"{card.FbDataId}"]!["prices"]!["ps"]!["updated"].GetValue<string>();
                        PriceLowest = jsonNod[$"{card.FbDataId}"]!["prices"]["ps"]!["LCPrice"].GetValue<string>();
                        PriceNext = jsonNod[$"{card.FbDataId}"]!["prices"]["ps"]!["LCPrice2"].GetValue<string>();
                        card.Tradable = !(Updated.Contains("Never") || (PriceLowest=="0" || PriceNext=="0"));
                        
                System.Console.WriteLine("Card: {0} - {1} with FbId {2} is tradable => {3}",card.Name,card.Revision,card.FbId,card.Tradable);
                    }
                    catch (Exception ex) { 
                        System.Console.WriteLine(ex.Message);
                        
                        }

                }
                
            

        }
          
       }
