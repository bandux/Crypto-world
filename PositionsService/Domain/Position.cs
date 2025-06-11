namespace PositionsService.Domain
{
    public enum PositionSide
    {
        Buy = 1,
        Sell = -1
    }

    public class Position
    {
        public int Id { get; set; }
        public string InstrumentId { get; set; } 
        public decimal Quantity { get; set; }
        public decimal InitialRate { get; set; }
        public PositionSide Side { get; set; }
        public decimal Value { get; set; }
    }
}