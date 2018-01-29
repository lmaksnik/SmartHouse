// SHWebServer.h

#ifndef _SHWEBSERVER_h
#define _SHWEBSERVER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif
#include <ESP8266WebServer.h>
#include <functional>


class SHResponse
{
public:
	SHResponse(const bool success, const String& data = "", const String& error = "")
		: success(success),
		data(data),
		error(error)
	{
	}

	bool success;
	String data;
	String error;
};

class SHWebServerClass
{
 protected:
	char* WifiSsid;

	String encrypt(String value);
	String decrypt(String value);

	bool validateHeader();
	String readInput();
	SHResponse callListeners(String commandName, String* parameters);


 public:
	void init();

	ESP8266WebServer Server;
	typedef std::function<SHResponse(String, String*)> TWebServerRequestFunction;

	void beginWifi(char* wifi_ssid, char* wifi_password);
	void beginServer(const int port, TWebServerRequestFunction handler);

	void handleClient();
	void onHandleRoot();
	void onHandleNotFound();
private:
	String _defaultCryptKey;
	TWebServerRequestFunction _handler;
	String* deserializeArgs(String input);
	String buildResponse(SHResponse data);
};

extern SHWebServerClass SHWebServer;


#endif

