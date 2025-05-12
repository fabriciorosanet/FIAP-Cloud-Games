using FCG.Application.WeatherForecast.Interfaces;
using FCG.Domain.WeatherForecast;
using FCG.Domain.WeatherForecast.Entities;
using FCG.Domain.WeatherForecast.Interfaces;
using Microsoft.Extensions.Logging;

namespace FCG.Application.WeatherForecast.Services {

	public class WeatherForecastService : IWeatherForecastService {
		
		private readonly IWeatherForecastRepository _repository;
		private readonly ILogger<WeatherForecastService> _logger;
		
		public WeatherForecastService(IWeatherForecastRepository repository, ILogger<WeatherForecastService> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		public async Task<WeatherForecastEntity[]> GetForecasts()
		{
			_logger.LogInformation("Buscando previsões do tempo...");
			var data = await _repository.GetForecasts();
			_logger.LogInformation("Previsões encontradas: {@Data}", data);
			return data;
		}
		
	}

}