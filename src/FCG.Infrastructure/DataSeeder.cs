using FCG.Domain.WeatherForecast.Entities;

namespace FCG.Infrastructure {

	public class DataSeeder {

		public static void Seed(AppDbContext context)
		{
			if (!context.WeatherForecasts.Any())
			{
				var summaries = new[] { "Freezing", "Chilly", "Warm", "Hot", "Scorching" };
				var forecasts = Enumerable.Range(1, 5).Select(index =>
					new WeatherForecastEntity()
					{
						Date = DateOnly.FromDateTime(DateTime.Today.AddDays(index)),
						TemperatureC = Random.Shared.Next(-20, 55),
						Summary = summaries[Random.Shared.Next(summaries.Length)]
					}).ToList();

				context.WeatherForecasts.AddRange(forecasts);
				context.SaveChanges();
			}
		}

	}

}