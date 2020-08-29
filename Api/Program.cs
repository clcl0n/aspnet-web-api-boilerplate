using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Api
{
	public class Program
	{
		public static int Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.File(
					formatter: new CompactJsonFormatter(),
					path: Path.Join(
						Directory.GetParent(
							Directory.GetCurrentDirectory()
						).FullName,
						"Logs/api.log"
					),
					fileSizeLimitBytes: long.MaxValue,
					rollOnFileSizeLimit: true,
					retainedFileCountLimit: 3
				)
				.CreateLogger();

			try
			{
				Log.Information("Starting web host");
				CreateHostBuilder(args).Build().Run();
				return 0;
			}
			catch (Exception exception)
			{
				Log.Fatal(exception, "Host terminated unexpectedly");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IWebHostBuilder CreateHostBuilder(string[] args)
		{
			IWebHostBuilder webHostBuilder = new WebHostBuilder();

			webHostBuilder.UseSerilog();
			webHostBuilder.UseKestrel();
			webHostBuilder.UseWebRoot(Directory.GetCurrentDirectory());
			webHostBuilder.ConfigureAppConfiguration((context, builder) =>
			   ConfigureConfiguration(context, builder)
			);
			webHostBuilder.UseStartup<Startup>();

			return webHostBuilder;
		}

		private static void ConfigureConfiguration(
			WebHostBuilderContext context,
			IConfigurationBuilder builder
		)
		{
			builder
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
		}
	}
}
