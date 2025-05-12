using FCG.Domain.WeatherForecast.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure {

	public class AppDbContext : DbContext {

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }

		public DbSet<WeatherForecastEntity> WeatherForecasts => Set<WeatherForecastEntity>();

	}

}