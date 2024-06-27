using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsNotifier;
using SportsResultsNotifierApp.Handlers;
using SportsResultsNotifierApp.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

host.Run();

static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    services.AddHostedService<SportsResultsBackgroundService>();
    services.AddSingleton<Scraper>();
    services.AddSingleton<Emailer>();
}