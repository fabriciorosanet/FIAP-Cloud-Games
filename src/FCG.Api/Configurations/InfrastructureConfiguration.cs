using FCG.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FCG.Api.Configurations {

	public static class InfrastructureConfiguration {

		public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(connectionString));
		}

	}

}