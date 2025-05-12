using FCG.Domain.WeatherForecast.Entities;
using FCG.Domain.WeatherForecast.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.WeatherForecast.Repositories {

	public class WeatherForecastRepository : IWeatherForecastRepository {

		private readonly AppDbContext _context;
		
		public WeatherForecastRepository(AppDbContext context) => _context = context;
		
		public async Task<WeatherForecastEntity[]> GetForecasts() 
		{
			return await _context.WeatherForecasts.ToArrayAsync();
		}
		
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};
		
	}

}