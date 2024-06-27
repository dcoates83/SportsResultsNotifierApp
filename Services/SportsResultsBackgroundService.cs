using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using SportsNotifier;
using SportsResultsNotifierApp.Handlers;

namespace SportsResultsNotifierApp.Services
{

    public class SportsResultsBackgroundService : BackgroundService
    {
        Emailer _emailer;
        Scraper _scrapper;
        TimeSpan _interval;
        public SportsResultsBackgroundService()
        {
        _scrapper = new Scraper();
        _emailer = new Emailer(
                "test@gmail.com", "password", "receiverEmail@gmail.com"
        );
        _interval = new TimeSpan(24, 0, 0);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var games = _scrapper.GetGames();
                    var message = "No games found.";
                    if (games.Item2.Count > 0)
                    {
                        message = "Games found: \n";
                        foreach (var game in games.Item2)
                        {
                            message += $"{game.Winner} beat {game.Loser} {game.WinnerScore} to {game.LoserScore}\n";
                        }
                    }
                    _emailer.SendEmail("NBA Results", message);
                    await Task.Delay(_interval);
                }
            });
        }
    }


}