using FCG.Api.Areas.Auth;
using FCG.Api.Areas.Usuarios.Endpoints;

namespace FCG.Api.Configurations {

	public static class EndpointsMapper {

		public static void EndpointsMap(this IEndpointRouteBuilder routes)
		{
			// Mapeia os endpoints
			UsuarioEndpointsMapper.Map(routes);
			AuthEndpoints.MapAuth(routes);
		}
	}

}