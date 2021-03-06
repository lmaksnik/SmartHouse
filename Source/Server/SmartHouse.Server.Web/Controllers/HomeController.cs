﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartHouse.Server.Core.WebScocket;
using SmartHouse.Server.Data.Model.Entities;
using SmartHouse.Server.Web.Models;
using SmartHouse.Server.Web.WebSocket;

namespace SmartHouse.Server.Web.Controllers {
	public class HomeController : Controller {

		protected ILogger<HomeController> Logger;

		public HomeController(ILogger<HomeController> logger) {
			Logger = logger;
		}

		public IActionResult Index() {
			Logger.Log(LogLevel.Critical, new EventId(1, "тестЛог"), this, null, (dictionary, exception) => { return exception?.Message ?? "sdsdsdsds"; });
			return View();
		}

		//public IActionResult Cnt() {
		//	using (var uow = new UnitOfWork()) {
		//		var obj = new TestItems(uow) {
		//			Name = "Test success name!",
		//			Is = true
		//		};
		//		obj.Save();
		//		uow.CommitChanges();

		//		var tt = uow.Query<TestItems>().ToList();
		//		return Content("count - " + tt.Count);
		//	}
		//}

		public IActionResult About() {
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact() {
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
