using FCG.Domain.WeatherForecast.Entities;

namespace FCG.Domain.WeatherForecast.Interfaces {

	public interface IWeatherForecastRepository {

		Task<WeatherForecastEntity[]> GetForecasts();

	}

}