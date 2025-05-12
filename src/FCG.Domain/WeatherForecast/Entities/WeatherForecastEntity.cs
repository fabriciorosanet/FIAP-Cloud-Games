using FCP.Domain.Base;

namespace FCG.Domain.WeatherForecast.Entities {

	public class WeatherForecastEntity : Entity {

		public DateOnly Date { get; set; }
		public int TemperatureC { get; set; }
		public string? Summary { get; set; }
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

	}

}