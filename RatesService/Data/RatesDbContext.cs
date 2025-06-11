using Microsoft.EntityFrameworkCore;
using RatesService.Models;

namespace RatesService.Data;

public class RatesDbContext : DbContext
{
	public RatesDbContext(DbContextOptions<RatesDbContext> options) : base(options) { }

	public DbSet<Rate> Rates => Set<Rate>();
}
