using FCG.Domain.WeatherForecast;
using FCG.Domain.WeatherForecast.Entities;

namespace FCG.Application.WeatherForecast.Interfaces {

	public interface IWeatherForecastService {

		Task<WeatherForecastEntity[]> GetForecasts();

	}

}