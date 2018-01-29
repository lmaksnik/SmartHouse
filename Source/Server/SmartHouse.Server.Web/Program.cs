using System;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SmartHouse.Server.Data.Model.Entities;

namespace SmartHouse.Server.Web {
	public class Program {
		public static void Main(string[] args) {
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseUrls("http://*:5000")
				.Build();
	}
}
