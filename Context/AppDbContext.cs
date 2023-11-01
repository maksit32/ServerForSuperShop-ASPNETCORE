using Microsoft.EntityFrameworkCore;
using Server.Entities;


namespace Server.Context
{
	public class AppDbContext : DbContext
	{
		//Список таблиц:
		public DbSet<Product> Products => Set<Product>();

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

	}
}
