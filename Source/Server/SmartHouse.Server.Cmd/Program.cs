using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketState = WebSocketSharp.WebSocketState;

namespace SmartHouse.Server.Cmd {
	class Program {
		static void Main(string[] args) {
			try {
				while (true) {
					Console.Write("Server host name or ip: ");
					var host = Console.ReadLine();
					if (string.IsNullOrWhiteSpace(host)) {
						Console.WriteLine("Invalid host name or ip!");
						continue;
					}
					RunWebSockets($"ws://{host.Trim('/')}/console").GetAwaiter().GetResult();
				}
			} catch (Exception ex) {
				Console.WriteLine(ex);
				Console.ReadKey();
			}

		//	new Program().OnStart(args);
			//Console.ReadKey();
		}
		private static async Task RunWebSockets(string url) {
			var client = new ClientWebSocket();
			await client.ConnectAsync(new Uri(url), CancellationToken.None);

			Console.WriteLine("Connected!");

			var sending = Task.Run(async () => {
				string line;
				while ((line = Console.ReadLine()) != null && line != String.Empty) {
					var bytes = Encoding.UTF8.GetBytes(line);

					await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
				}

				await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
			});

			var receiving = Receiving(client);

			await Task.WhenAll(sending, receiving);
		}

		private static async Task Receiving(ClientWebSocket client) {
			var buffer = new byte[1024 * 4];

			while (true) {
				var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

				if (result.MessageType == WebSocketMessageType.Text)
					Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));

				else if (result.MessageType == WebSocketMessageType.Close) {
					await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
					break;
				}
			}
		}






		protected bool WaitResponse = false;
		public async Task OnStart(string[] args) {
			try {
				Console.Write("Server host name or ip: ");
				var host = Console.ReadLine();
				using (var webSocket = new ClientWebSocket()) {
					await webSocket.ConnectAsync(new Uri($"ws://{host?.Trim('/')}/console"), CancellationToken.None);
					while (true) {
						try {
							if (webSocket.State == System.Net.WebSockets.WebSocketState.Connecting) {
								Console.WriteLine("Web socket connecting ...");
								Thread.Sleep(1000);
								continue;
							}
							if (webSocket.State == System.Net.WebSockets.WebSocketState.Closed) {
								Console.WriteLine("Web socket closed!");
								break;
							}
							if (webSocket.State == System.Net.WebSockets.WebSocketState.CloseSent) {
								Console.WriteLine("Web socket closing ...");
								Thread.Sleep(1000);
								continue;
							}
							if (webSocket.State == System.Net.WebSockets.WebSocketState.Open) {
								Console.Write("websocket:> ");
								var line = Console.ReadLine();
								if (!string.IsNullOrWhiteSpace(line)) {
									if (line.ToLower() == "exit") break;
									await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(line)),WebSocketMessageType.Text, false, CancellationToken.None);
									
									var buffer = new byte[1024 * 4];
									await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
									Console.WriteLine($"[{DateTime.Now:dd.MM.YYYY HH:mm:ss}] {Encoding.UTF8.GetString(buffer)}");
								}
							}

						} catch (Exception ex) {
							Console.WriteLine(JsonConvert.SerializeObject(ex));
						}
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(JsonConvert.SerializeObject(ex));
			}
			Console.ReadKey();
		}

		private static void WebSocketOnMessage(object sender, MessageEventArgs messageEventArgs) {
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.WriteLine($"[{DateTime.Now:dd.MM.YYYY HH:mm:ss}] {messageEventArgs.Data}");
			//WaitResponse = false;
		}


	}
}
