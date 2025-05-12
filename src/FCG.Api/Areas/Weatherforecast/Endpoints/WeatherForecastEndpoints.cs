using FCG.Application.WeatherForecast.Interfaces;

namespace FCG.Api.Areas.Weatherforecast.Endpoints {

	public static class WeatherForecastEndpoints
	{
		public static void MapWeatherForecastEndpoints(this IEndpointRouteBuilder routes)
		{

			routes.MapGet("/weatherforecast/climates", async (IWeatherForecastService service, ILoggerFactory loggerFactory) =>
			{
				var logger = loggerFactory.CreateLogger("WeatherForecastEndpoint");
				logger.LogInformation("Requisição recebida para /weatherforecast/climates");
				
				try
				{
					var forecasts = await service.GetForecasts();
					return Results.Ok(forecasts);
				} 
				catch (Exception ex)
				{
					logger.LogError(ex, "Erro ao obter previsões.");
					return Results.Problem("Erro interno ao buscar previsão do tempo.");
				}

			})
			.WithName("GetClimatesWeatherForecast")
			.WithSummary("Retorna a lista de previsões do tempo de forma assíncrona.");;
		}
	}

}