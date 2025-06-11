namespace Contracts;

public class RateChangedEvent
{
    public string InstrumentId { get; set; } = default!;
    public string Symbol { get; set; } = default!;
    public decimal OldRate { get; set; }
    public decimal NewRate { get; set; }
    public DateTime Timestamp { get; set; }
}
