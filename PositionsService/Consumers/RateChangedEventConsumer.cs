using MassTransit;
using Contracts;
using PositionsService.Persistence;
using System.Linq;

namespace PositionsService.Consumers
{
    public class RateChangedEventConsumer : IConsumer<RateChangedEvent>
    {
        private readonly PositionsDbContext _dbContext;

        public RateChangedEventConsumer(PositionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<RateChangedEvent> context)
        {
            var message = context.Message;
            Console.WriteLine($"[CONSUMER] Received: {message.Symbol} changed from {message.OldRate} to {message.NewRate}");
            var instrumentId = context.Message.InstrumentId;
            var newRate = context.Message.NewRate;

            var positions = _dbContext.Positions
                .Where(p => p.InstrumentId == instrumentId)
                .ToList();

       
            foreach (var position in positions)
            {
                position.Value = position.Quantity * newRate;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
