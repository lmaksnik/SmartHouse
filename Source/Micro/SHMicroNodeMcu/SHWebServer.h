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

class JsonClass
{
public:
	void beginObject() {
		_str += "{";
	}
	void endObject() {
		if (_str.endsWith(","))
			_str.remove(_str.length() - 1, 1);
		_str += "}";
	}
	void addProperty(String name) {
		if (name != nullptr && name.length() > 0)
			_str += "\"" + name + "\":";
	}
	void addPropertyValue(const String value) {
		if (value == nullptr)
			_str += "null,";
		else _str += "\"" + value + "\",";
	}
	void addPropertyValue(char* value) {
		_str += String(value) + ",";
	}
	void addPropertyValue(const float value) {
		_str += String(value) + ",";
	}
	void addPropertyValue(bool value) {
		if (value)
			_str += "true,";
		else
			_str += "false,";
	}
	void addPropertyJObject(const String jsonObject) {
		if (jsonObject == nullptr)
			_str += "null,";
		else
			_str += jsonObject + ",";
	}
	String toString() const {
		return _str;
	}
protected:
	String _str = "";
};

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

	String toJson() const
	{
		JsonClass json = JsonClass();
		json.beginObject();
		json.addProperty("success");
		json.addPropertyValue(this->success);
		json.addProperty("data");
		if (data != nullptr && data.startsWith("{"))
			json.addPropertyJObject(this->data);
		else
			json.addPropertyValue(this->data);
		json.addProperty("error");
		json.addPropertyValue(this->error);
		json.endObject();
		return json.toString();
	}
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

