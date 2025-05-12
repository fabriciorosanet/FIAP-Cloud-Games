namespace FCG.Api.Areas.Weatherforecast.Endpoints {

	public static class WeatherForecastEndpointsMapper {

		public static void Map(IEndpointRouteBuilder routes)
		{
			routes.MapWeatherForecastEndpoints();
		}

	}

}