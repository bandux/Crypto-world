using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RatesService.Data;
using RatesService.Services;
using Contracts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});


builder.Services.AddDbContext<RatesDbContext>(opt => opt.UseInMemoryDatabase("RatesDb"));
builder.Services.AddHttpClient<CoinMarketCapClient>();
builder.Services.AddScoped<RateService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/trigger-fetch", async (RateService rateService) =>
{
    await rateService.FetchAndCompareRatesAsync();
    return Results.Ok("Rates fetched and compared.");
});

app.Run();
