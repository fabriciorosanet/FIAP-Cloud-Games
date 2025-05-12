using Serilog;
using Serilog.Events;

namespace FCG.Api.Configurations.Serilog {

	public class LoggingConfiguration {

		public static void ConfigureLogging(WebApplicationBuilder builder)
		{
			var logFolder = Path.Combine("Logs", DateTime.Now.ToString("yyyy-MM-dd"));
			Directory.CreateDirectory(logFolder);

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("System", LogEventLevel.Warning)
				.MinimumLevel.Error()
				.Enrich.FromLogContext()
				.Enrich.WithCorrelationId()
				.WriteTo.Console()
				.WriteTo.File(
					path: Path.Combine(logFolder, "errors.log"),
					rollingInterval: RollingInterval.Day,
					retainedFileCountLimit: 30,
					restrictedToMinimumLevel: LogEventLevel.Error,
					outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}"
				)
				.CreateLogger();
			
			builder.Host.UseSerilog();
		}

	}

}