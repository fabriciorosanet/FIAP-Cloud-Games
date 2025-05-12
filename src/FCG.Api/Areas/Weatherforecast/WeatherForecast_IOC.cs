using FCG.Application.WeatherForecast.Interfaces;
using FCG.Application.WeatherForecast.Services;
using FCG.Domain.WeatherForecast.Interfaces;
using FCG.Infrastructure.WeatherForecast.Repositories;

namespace FCG.Api.Areas.WeatherForecast {

	public static class WeatherForecast_IOC {

		public static void AddApplicationServices(IServiceCollection services)
		{
			services.AddScoped<IWeatherForecastService, WeatherForecastService>();
			services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

		}

	}

}