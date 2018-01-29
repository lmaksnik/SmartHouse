// 
// 
// 

#include "SHWebServer.h"


const String HEADERAUTHORIZATION = "Authorization";
const String ARGCMD = "cmd";

void handleRoot()
{
	SHWebServer.onHandleRoot();
}

void handleNotFound()
{
	SHWebServer.onHandleNotFound();
}

void SHWebServerClass::beginWifi(char* wifi_ssid, char* wifi_password)
{
	this->WifiSsid = wifi_ssid;
	WiFi.begin(wifi_ssid, wifi_password);

	Serial.println("");
	Serial.printf("Connecting WIFI \"%s\"", this->WifiSsid);
	while (WiFi.status() != WL_CONNECTED) {
		delay(500);
		Serial.print(".");
	}
	Serial.println(" successfully!");

	Serial.print("IP address: ");
	Serial.println(WiFi.localIP());
	Serial.printf("ChipId: %08X\n", ESP.getChipId());
}

void SHWebServerClass::beginServer(const int port, TWebServerRequestFunction handler)
{
	Serial.println("");
	Serial.print("Web server starting ...");

	if (handler == nullptr)
	{
		Serial.println(" failure!");
		Serial.println("Handler not found");
	}
	_handler = handler;

	this->Server = ESP8266WebServer(port);

	this->Server.on("/", handleRoot);

	this->Server.onNotFound(handleNotFound);

	this->Server.begin();
	Serial.println(" successfully!");
}

void SHWebServerClass::handleClient()
{
	this->Server.handleClient();
}

void SHWebServerClass::onHandleRoot()
{
	SHResponse response(false);
	if (validateHeader()){
		String input = readInput();
		if (input != nullptr && input.length() > 0)
		{
			input = decrypt(input);
			int idx = input.indexOf(' ');
			String cmdName = input.substring(0, idx);
			if (cmdName == nullptr || cmdName.length() < 1)
			{
				cmdName = input;
			}
			input.remove(0, idx + 1);
			String* args = deserializeArgs(input);
			response = callListeners(cmdName, args);
		}
	} else
	{
		response = SHResponse(false, "", "security");
	}

	const String result = encrypt(buildResponse(response));
	Server.send(200, "text/json", result);
}

void SHWebServerClass::onHandleNotFound()
{
	SHResponse response(false);
	const String result = encrypt(buildResponse(response));
	Server.send(200, "text/json", result);
}

String* SHWebServerClass::deserializeArgs(String input)
{
	return nullptr;
}

String SHWebServerClass::buildResponse(SHResponse response)
{
	String str = "{success: ";
	if (response.success)
	{
		str += "true";
	} else
	{
		str += "false";
	}
	str += ", data:\"" + response.data + "\", error:\"" + response.error+ "\", crypto: \"SDFKJDFKJDHF\"}";
	return str;
}

bool SHWebServerClass::validateHeader()
{
	if (Server.args() > 0)
	{
		String val = Server.header(HEADERAUTHORIZATION);
		if (val != nullptr && val.length() > 0 && val == "true")
		{
			return true;
		}
	}
	return false;
}

String SHWebServerClass::readInput()
{
	if (Server.args() > 0 && Server.hasArg(ARGCMD))
	{
		return Server.arg(ARGCMD);
	}
	return "";
}

String SHWebServerClass::encrypt(String value)
{
	return value;
}

String SHWebServerClass::decrypt(String value)
{
	return value;
}

SHResponse SHWebServerClass::callListeners(const String commandName, String* parameters)
{
	if (_handler != nullptr)
	{
		return _handler(commandName, parameters);
	}
	return SHResponse(false);
}

void SHWebServerClass::init(){}


SHWebServerClass SHWebServer;


/*String result = "";
result = result + "Server.hostHeader - ";
result = result + Server.hostHeader() + "\n";

result = result + "Server.uri - ";
result = result + Server.uri() + "\n";

if (Server.args() > 0)
{
result = result + "Request arguments:\n";
for (int i = 0; i < Server.args(); ++i)
{
String argName = Server.argName(i);
String argValue = Server.arg(i);

result = result + "    " + argName + " - ";

result = result + argValue + "\n";

}
}

if (Server.headers() > 0)
{
result = result + "Headers:\n";
for (int i = 0; i < Server.headers(); ++i)
{
String argName = Server.headerName(i);
String argValue = Server.header(i);

result = result + "    " + argName + " - ";

result = result + argValue + "\n";

}
}
Server.send(200, "text/plain", "onHandleRoot\n" + result);*/