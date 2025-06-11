using Microsoft.EntityFrameworkCore;
using PositionsService.Persistence;
using PositionsService.Domain;
using PositionsService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PositionsDbContext>(options =>
    options.UseInMemoryDatabase("PositionsDb"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<RateChangedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("rate-changed-queue", e =>
        {
            e.ConfigureConsumer<RateChangedEventConsumer>(context);
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PositionsDbContext>();
    SeedPositions(db);
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "PositionsService is running.");

app.Run();

void SeedPositions(PositionsDbContext db)
{
    if (!db.Positions.Any())
    {
        db.Positions.AddRange(
            new Position { InstrumentId = "BTC/USD", Quantity = 1, InitialRate = 58000, Side = PositionSide.Buy },
            new Position { InstrumentId = "ETH/USD", Quantity = 2, InitialRate = 4000, Side = PositionSide.Sell }
        );
        db.SaveChanges();
    }
}
