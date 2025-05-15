using FCG.Api.Areas.Usuarios;

namespace FCG.Api.Configurations {

	public static class DependencyInjection {

		public static void AddApplicationServices(this IServiceCollection services)
		{
			Usuarios_IOC.AddApplicationServices(services);
		}

	}

}