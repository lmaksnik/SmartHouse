using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestDevices {
	class Program {
		static void Main(string[] args) {
			var ipAddres = "192.168.1.118";
			var port = 80;
			var url = $"http://{ipAddres}:{port}/";
			Console.WriteLine(url);

			while (true) {
				Console.Write("cmd>");
				var input = Console.ReadLine()?.ToLower();
				if (input == "close") break;

				try {
					if (string.IsNullOrWhiteSpace(input)) continue;
					using (var wc = new MyWebClient()) {
						wc.Headers.Add("Authorization", "true");
						
						wc.Encoding = Encoding.UTF8;
						var result = wc.UploadString($"{url}?cmd={input}", "POST", "");
						Console.WriteLine("---------------------------------------------------------------");
						Console.WriteLine(result);
						Console.WriteLine("---------------------------------------------------------------");
					}
				} catch (Exception ex) {
					Console.WriteLine(ex);
				}
			}
		}

		private class MyWebClient : WebClient {
			protected override WebRequest GetWebRequest(Uri uri) {
				WebRequest w = base.GetWebRequest(uri);
				w.Timeout = 1 * 20 * 1000;
				return w;
			}
		}
	}
}
