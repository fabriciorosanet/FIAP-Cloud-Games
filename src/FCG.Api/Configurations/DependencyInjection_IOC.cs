using FCG.Api.Areas.WeatherForecast;

namespace FCG.Api.Configurations {

	public static class DependencyInjection {

		public static void AddApplicationServices(this IServiceCollection services)
		{
			WeatherForecast_IOC.AddApplicationServices(services);

		}

	}

}