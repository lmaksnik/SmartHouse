
#include "SHWebSocket.h"
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WebSocketsClient.h>

void SHWebSocketClass::init()
{
}

void SHWebSocketClass::beginWifi(String ssid, String password)
{
	WiFiMulti.addAP("ORC", "Pass@orc");

	//WiFi.disconnect();
	while (WiFiMulti.run() != WL_CONNECTED) {
		delay(100);
	}
}

void SHWebSocketClass::webSocketEvent(const WStype_t type, uint8_t * payload, const unsigned long length) {
	switch (type) {
		case WStype_DISCONNECTED:
			Serial.printf("[WSc] Disconnected!\n");
			break;
		case WStype_CONNECTED: {
				Serial.printf("[WSc] Connected to url: %s\n", payload);

				// send message to server when Connected
				webSocket.sendTXT("Connected");
			}
			break;
		case WStype_TEXT:
			Serial.printf("[WSc] get text: %s\n", payload);
			// send message to server
			// webSocket.sendTXT("message here");
			break;
		case WStype_BIN:
			Serial.printf("[WSc] get binary length: %u\n", length);
			hexdump(payload, length);

			// send data to server
			// webSocket.sendBIN(payload, length);
			break;
	}
}

void SHWebSocketClass::beginWebSocket(String host, uint16 port, String url, String protocol, const char* user, const char* password)
{
	// server address, port and URL
	webSocket.begin(host, port, url, protocol);
	// event handler
	webSocket.onEvent(this->webSocketEvent);
	// use HTTP Basic Authorization this is optional remove if not needed
	webSocket.setAuthorization(user, password);
	// try ever 5000 again if connection has failed
	webSocket.setReconnectInterval(3000);
	webSocket.sendTXT("hello");
}

void SHWebSocketClass::webSocketLoop()
{
	webSocket.loop();
}


SHWebSocketClass SHWebSocket;

