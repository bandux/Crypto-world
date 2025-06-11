public class RecalculatedPositionEvent
{
    public int PositionId { get; set; }
    public string InstrumentId { get; set; }
    public decimal Quantity { get; set; }
    public decimal InitialRate { get; set; }
    public decimal CurrentRate { get; set; }
    public string Side { get; set; }
    public decimal ProfitLoss { get; set; }
}
