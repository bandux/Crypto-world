namespace RatesService.Models;

public class Rate
{
    public int Id { get; set; }
    public string Symbol { get; set; } = null!;
    public decimal Value { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
