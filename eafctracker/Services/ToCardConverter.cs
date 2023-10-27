using Eafctracker.Models;
using HtmlAgilityPack;

namespace Eafctracker.Services;

public class ToCardConverter : IToCardConverter
{
    public Card GetCardFromPersonalPage(HtmlDocument doc)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Card> GetCardFromListOfPlayersPage()
    {
        int nodesIndex = 0;

    }

    private int GetNumberPages(HtmlDocument doc)
        => int.Parse(doc.DocumentNode.SelectSingleNode("//nav/ul/li[last()-1]/a").InnerText);
    
    private   string? ParseFromDoc(HtmlDocument page,string xPath)
        =>  page.DocumentNode
            .SelectSingleNode(xPath)
            .InnerText;
    private async IAsyncEnumerable<Card> GetCardsAsync (HtmlDocument doc)
    {
        int nodesIndex = 0;
        List<(string?, string?, string?, string?, string?)> users = new();
        HtmlNodeCollection? link = doc.DocumentNode.SelectNodes("//a[@class='player_name_players_table get-tp']");
        for (int i = 1; i <= 61; i += 2)
        {
            if (i == 21 || i == 42)
                i++;
            yield return new Card()
            {
                FbId = Int32.Parse(link[nodesIndex++].GetAttributeValue("data-site-id", "")),
                Name = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[2]/div[2]/div[1]/a"),
                Rating = Int32.Parse( ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[3]/span")),
                Position = ParseFromDoc(doc, $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[4]/div[1]"),
                Revision = ParseFromDoc(doc,
                    $"//*[@id=\"repTb\"]/tbody/tr[{i}]/td[5]/div[1]"),
                
            };
        }
    }
}

public interface IToCardConverter
{
    public Card GetCardFromPersonalPage(HtmlDocument page);
    public Card GetCardFromListOfPlayersPage(HtmlDocument page);
    
}