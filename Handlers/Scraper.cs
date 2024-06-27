using HtmlAgilityPack;
using SportsResultsNotifierApp.Models;
namespace SportsNotifier;

public class Scraper
{
    private string _url = "https://www.basketball-reference.com/boxscores/";
    private List<Game> games = new();

    public Scraper()
    {
    }

    public void SetScrapeUrl(string url)
    {
        _url = url;
    }

    public Tuple<string, List<Game>> GetGames()
    {
        HtmlWeb web = new();
        HtmlDocument doc = web.Load(_url);
        try
        {
            var titleNode = doc.GetElementbyId("content")?.SelectSingleNode("//h1");
            var title = titleNode?.InnerText ?? "No title found";

            var resultTableNodes = doc.DocumentNode.SelectNodes("//div[@class='game_summary expanded nohover ']");

            if (resultTableNodes != null)
            {
                foreach (var node in resultTableNodes)
                {
                    var winnerNode = node.SelectSingleNode(".//table/tbody/tr[1]/td[1]");
                    var loserNode = node.SelectSingleNode(".//table/tbody/tr[2]/td[1]");
                    var winnerScoreNode = node.SelectSingleNode(".//table/tbody/tr[1]/td[2]");
                    var loserScoreNode = node.SelectSingleNode(".//table/tbody/tr[2]/td[2]");

                    if (winnerNode != null && loserNode != null && winnerScoreNode != null && loserScoreNode != null)
                    {
                        games.Add(new Game
                        {
                            Winner = winnerNode.InnerText,
                            Loser = loserNode.InnerText,
                            WinnerScore = winnerScoreNode.InnerText,
                            LoserScore = loserScoreNode.InnerText
                        });
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("No game summaries found.");
            }

            return Tuple.Create(title, games);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"There was an error getting data from the web: {ex.Message}");
            throw;
        }
    }
}
