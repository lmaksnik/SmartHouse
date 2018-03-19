using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Mx.Common.WebScocket;
using SmartHouse.Server.Common;
using SmartHouse.Server.Core.WebScocket;

namespace SmartHouse.Server.Web.WebSocket {
	public static class Extensions {

		public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path, WebSocketHandler handler) {
			return app.Map(path, (_app) => _app.UseMiddleware<WebSocketMiddleware>(handler));
		}

		public static IServiceCollection AddWebSocketManager(this IServiceCollection services) {
			services.AddTransient<WebSocketConnectionManager>();

			foreach (var assembly in Global.AllAssemblies) {
				foreach (var type in assembly.ExportedTypes) {
					if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler)) {
						services.AddSingleton(type);
					}
				}
			}

			return services;
		}

	}
}
