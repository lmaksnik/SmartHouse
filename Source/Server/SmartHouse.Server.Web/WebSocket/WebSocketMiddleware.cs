﻿using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mx.Common.WebScocket;
using SmartHouse.Server.Core.WebScocket;

namespace SmartHouse.Server.Web.WebSocket {
	public class WebSocketMiddleware {
		private readonly RequestDelegate _next;

		private WebSocketHandler _webSocketHandler { get; set; }

		public WebSocketMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler) {
			_next = next;
			_webSocketHandler = webSocketHandler;
		}

		public async Task Invoke(HttpContext context) {
			if (!context.WebSockets.IsWebSocketRequest)
				return;

			var socket = await context.WebSockets.AcceptWebSocketAsync();
			await _webSocketHandler.OnConnected(socket);

			await Receive(socket, async (result, buffer) => {
				if (result.MessageType == WebSocketMessageType.Text) {
					await _webSocketHandler.ReceiveAsync(socket, result, buffer);
					return;
				} else if (result.MessageType == WebSocketMessageType.Close) {
					await _webSocketHandler.OnDisconnected(socket);
					return;
				}

			});

			//TODO - investigate the Kestrel exception thrown when this is the last middleware
			//await _next.Invoke(context);
		}

		private async Task Receive(System.Net.WebSockets.WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage) {
			var buffer = new byte[1024 * 4];

			while (socket.State == WebSocketState.Open) {
				var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
					cancellationToken: CancellationToken.None);

				handleMessage(result, buffer);
			}
		}
	}
}
