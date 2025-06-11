using Microsoft.EntityFrameworkCore;
using Contracts;
using RatesService.Data;
using RatesService.Models;
using MassTransit;

namespace RatesService.Services;

public class RateService
{
    private readonly RatesDbContext _db;
    private readonly CoinMarketCapClient _client;
    private readonly IPublishEndpoint _publishEndpoint;

    public RateService(RatesDbContext db, CoinMarketCapClient client, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _client = client;
        _publishEndpoint = publishEndpoint;
    }

    public async Task FetchAndCompareRatesAsync()
    {
        var newRates = await _client.GetLatestRatesAsync();

        foreach (var (symbol, newValue) in newRates)
        {
            var oldRate = await _db.Rates
                .Where(r => r.Symbol == symbol && r.Timestamp >= DateTime.UtcNow.AddHours(-24))
                .OrderBy(r => r.Timestamp)
                .FirstOrDefaultAsync();

            if (oldRate != null)
            {
                var change = Math.Abs((newValue - oldRate.Value) / oldRate.Value);
                if (change > 0.05m)
                {
                    Console.WriteLine($"Rate change detected for {symbol}: old={oldRate.Value}, new={newValue}, change={change:P2}. Publishing message...");

                    var evt = new RateChangedEvent
                    {
                        Symbol = symbol,
                        OldRate = oldRate.Value,
                        NewRate = newValue,
                        Timestamp = DateTime.UtcNow
                    };

                    await _publishEndpoint.Publish(evt);
                    Console.WriteLine($"[EVENT] Published: {symbol} from {oldRate.Value} → {newValue}");
                }
            }

            _db.Rates.Add(new Rate
            {
                Symbol = symbol,
                Value = newValue,
                Timestamp = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
    }
}
