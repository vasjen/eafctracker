using System.Net;
using Eafctracker.Models;
using Eafctracker.Services;
using Eafctracker.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Eafctracker.Controllers;

[ApiController]
[Route("[controller]")]
public class ProxyController (IHttpClientService webService) : ControllerBase
{
    private readonly IHttpClientService _webService = webService;
    [HttpGet]
    public async Task<IActionResult> GetProxiesList()
    {
        HttpClient client = webService.GetClient();
        string page = await client.GetStringAsync(
            "https://www.futbin.com/players?page=1&player_rating=82-99&ps_price=15000-15000000");
        
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(page);
        int nodesIndex = 0;
        List<(string?,string?, string?, string?, string?)> users = new();
        for (int i = 1; i <= 61; i += 2) 
        {
            if (i == 21 || i == 42)
                i++;
            
            string? name = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[2]/div[2]/div[1]/a");
            Console.WriteLine(name);
            string? rating = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[3]/span");
            Console.WriteLine(rating);
            string? dataSiteId = String.Empty;
                HtmlNodeCollection? link = doc.DocumentNode.SelectNodes("//a[@class='player_name_players_table get-tp']");
                
                // HtmlNode link = doc.DocumentNode.SelectSingleNode($"/html/body/div[9]/div[2]/div[5]/div[3]/table/tbody/tr[{i}]/td[2]/div[2]/div[1]/a");
              
                    dataSiteId = link[nodesIndex++].GetAttributeValue("data-site-id", "");
               

                Console.WriteLine(dataSiteId);
                string? position = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[4]/div[1]");
                string? revision = ParseFromDoc(doc,
                    $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[5]/div[1]");
                Console.WriteLine(revision);
                Console.WriteLine(position+"\n");
            users.Add((name,rating,dataSiteId,revision,position));
             
            
        }
        // while (ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{index.ToString()}]/td[2]/div[2]/div[1]/a") is not null)
        // {
            // names.Add(index,ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{index.ToString()}]/td[2]/div[2]/div[1]/a"));
            // index += 2;
        // }
        string namePele = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[1]/td[2]/div[2]/div[1]/a");
        string nameMuller = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[22]/td[2]/div[2]/div[1]/a");
        // string nameZizu = ParseFromDoc(doc, "//*[@id=\"repTb\"]/tbody/tr[3]/td[2]/div[2]/div[1]/a");
        //a[contains(@aria-describedby,'tippy-1')]
        // Console.WriteLine(nameZizu);
        
        return Ok();
    }

    [HttpGet]
    [Route("number")]
    public async Task<IActionResult> GetNumber()
    {
        HttpClient client = webService.GetClient();
        string page = await client.GetStringAsync(
            "https://www.futbin.com/players?page=1&player_rating=82-99&ps_price=15000-15000000");
        
        var doc = new HtmlDocument();
        doc.LoadHtml(page);
        var link = doc.DocumentNode.SelectSingleNode("//nav/ul/li[last()-1]/a");
        return Ok(link.InnerText);
    }
    string? ParseFromDoc(HtmlDocument page,string xPath)
        =>  page.DocumentNode
            .SelectSingleNode(xPath)
            .InnerText;
}