using Microsoft.EntityFrameworkCore;
using PositionsService.Domain;

namespace PositionsService.Persistence
{
	public class PositionsDbContext : DbContext
	{
		public DbSet<Position> Positions { get; set; }

		public PositionsDbContext(DbContextOptions<PositionsDbContext> options) : base(options) { }
	}
}