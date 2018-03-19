// SHWebSocket.h

#ifndef _SHWEBSOCKET_h
#define _SHWEBSOCKET_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif
#include "WebSocketsClient.h"
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

class SHWebSocketClass
{
protected:
	

public:
	void init();
	void beginWifi(String ssid, String password);
	void beginWebSocket(String host, uint16 port, String url, String protocol, const char* user, const char* password);
	void webSocketLoop();
	ESP8266WiFiMulti WiFiMulti;
	WebSocketsClient webSocket;
	void webSocketEvent(WStype_t type, uint8_t* payload, unsigned long length);
};

extern SHWebSocketClass SHWebSocket;

#endif

