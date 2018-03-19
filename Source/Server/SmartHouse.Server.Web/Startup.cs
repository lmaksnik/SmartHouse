using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHouse.Server.Core.Logger;
using SmartHouse.Server.Core.WebScocket;
using SmartHouse.Server.Web.WebSocket;

namespace SmartHouse.Server.Web {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddLogging();
			services.AddWebSocketManager();
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
			if (env.IsDevelopment()) {
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
			}
			//app.UseStaticFiles();
			loggerFactory.AddProvider(new LoggerProviderFile(Configuration.GetSection("Logging"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")));


			app.UseWebSockets();
			app.MapWebSocketManager("/sensor", app.ApplicationServices.GetService<WebSocketHandlerSensor>());
			app.MapWebSocketManager("/console", app.ApplicationServices.GetService<WebSocketHandlerConsole>());

			//app.UseDbXpoPostgres(Configuration.GetConnectionString("sh_cs"), AutoCreateOption.DatabaseAndSchema, typeof(DbObject).Assembly);

			app.UseMvc(routes => {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
