namespace PositionsService.Contracts
{
    public class AddNewPositionEvent
    {
        public string InstrumentId { get; set; }
        public decimal Quantity { get; set; }
        public decimal InitialRate { get; set; }
        public string Side { get; set; }
    }
}
