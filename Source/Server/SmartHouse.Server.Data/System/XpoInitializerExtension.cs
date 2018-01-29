using System;
using System.Reflection;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHouse.Server.Data.System {
	public static class XpoInitializerExtension {
		/// <summary>
		/// Инициализация подключения к базе данных PostgreSQL
		/// </summary>
		/// <param name="app"></param>
		/// <param name="connectionString">Строка подключения к базе данных</param>
		/// <param name="autoCreateOption"></param>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		public static IApplicationBuilder UseDbXpoPostgres(this IApplicationBuilder app, string connectionString, AutoCreateOption autoCreateOption, params Assembly[] assemblies) {
			if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
			var parser = new ConnectionStringParser(connectionString);
			var dbType = parser.GetPartByName("XpoProvider");
			if (string.IsNullOrWhiteSpace(dbType)) {
				parser.RemovePartByName("XpoProvider");
				parser.AddPart("XpoProvider", "Postgres");
			}
			var init = new XpoInitializer(parser.GetConnectionString(), autoCreateOption, assemblies);
			init.Init();
			return app;
		}
	}
}
