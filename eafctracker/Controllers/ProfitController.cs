using Eafctracker.Models;
using Eafctracker.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eafctracker.Controllers;

[ApiController]
[Route("[controller]")]

public class ProfitController : ControllerBase
{
    private readonly IProfitService _profitService;
    private readonly IHttpClientService _httpClientService;

    public ProfitController(IProfitService profitService, IHttpClientService httpClientService)
    {
        _profitService = profitService;
        _httpClientService = httpClientService;
    }
    
    
    [HttpGet]
    [ProducesResponseType(typeof(Card), 200)]
    public async Task<IActionResult> Get(int id)
    {
        // await _profitService.FindingProfitAsync();
        return Ok();
    }

    [HttpGet]
    [Route("test")]
    public async Task<IActionResult> Test(int page = 1, int minPrice = 20000)
    {
        var link = $"https://www.futbin.com/players?page={page}&ps_price={minPrice}-15000000";
        // var client = new HttpClient();
        // client.DefaultRequestHeaders.Add("User-Agent", "User browser");
        // var resultPage = await client.GetStringAsync(link);
        // HtmlDocument doc = new HtmlDocument();
        // doc.Load(resultPage);
        // HtmlNode tbody = doc.DocumentNode.SelectSingleNode("//tbody");
        // Console.Clear();
        // Console.WriteLine(tbody.SelectNodes(".//tr").Count);
        // var playersOnPageCount = tbody.SelectNodes(".//tr").Count;

        HtmlWeb web = new HtmlWeb();
        var doc1 = web.Load(link);
        HtmlNodeCollection? playerVer1Info = doc1.DocumentNode.SelectNodes("//tr[@class='player_tr_1']");
        HtmlNodeCollection? playerVer2Info = doc1.DocumentNode.SelectNodes("//tr[@class='player_tr_2']");
     
        if (playerVer1Info is null || playerVer2Info is null)
            return NotFound("No players on page");

        while (playerVer1Info is not null  && page <= 4)
        {
          
           
            page++;
        }
        
        return Ok($"Total players on page: {playerVer1Info.Count + playerVer2Info.Count}");
        
        
        
        
        
        
        
        
        
        
        
        
        // return Ok($"Total players on page: {playersOnPageCount}");
        var document = GetHtmlDocument(link);
        var pagesNumber = ParseFromDoc(await document,
            "(//a[contains(@class,'page-link  waves-effect waves-effect')])[6]");
        return Ok($"Number of pages: {pagesNumber}");

    }
    
    private string ParseFromDoc(HtmlDocument page,string xPath)
        =>  page.DocumentNode
            .SelectSingleNode(xPath)
            .InnerText;
    private async Task<HtmlDocument?> GetHtmlDocument(string link)
    {
        // var client = _httpClientService.GetHttpClient();
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "User browser");
        var response = await client.GetStringAsync(link);
            
        var doc = new HtmlDocument();
        doc.LoadHtml(response);
        return doc;
    }   
    
}