using MassTransit;
using PositionsService.Contracts;
using PositionsService.Persistence;
using PositionsService.Domain;
namespace PositionsService.Consumers
{
    public class AddNewPositionConsumer : IConsumer<AddNewPositionEvent>
    {
        private readonly PositionsDbContext _db;

        public AddNewPositionConsumer(PositionsDbContext db) => _db = db;

        public async Task Consume(ConsumeContext<AddNewPositionEvent> context)
        {
            var msg = context.Message;
            var position = new Position
            {
                InstrumentId = msg.InstrumentId,
                Quantity = msg.Quantity,
                InitialRate = msg.InitialRate,
                Side = Enum.Parse<PositionSide>(msg.Side, true)
            };

            _db.Positions.Add(position);
            await _db.SaveChangesAsync();
        }
    }
}