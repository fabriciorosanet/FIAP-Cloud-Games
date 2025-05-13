using FCG.Api.Areas.Usuarios.Endpoints;
using FCG.Api.Areas.Weatherforecast.Endpoints;

namespace FCG.Api.Configurations {

	public static class EndpointsMapper {

		public static void EndpointsMap(this IEndpointRouteBuilder routes)
		{
			// Mapeia os endpoints
			WeatherForecastEndpointsMapper.Map(routes);
			UsuarioEndpointsMapper.Map(routes);
		}
	}

}