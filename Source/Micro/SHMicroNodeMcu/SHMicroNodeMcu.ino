/*
 Name:		SHMicroNodeMcu.ino
 Created:	1/25/2018 7:54:24 PM
 Author:	m
*/

#include "SHWebSocket.h"
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WebSocketsClient.h>
/*
#include "DHT.h"
#define DHTPIN 4    // modify to the pin we connected
// Uncomment whatever type you're using!
//#define DHTTYPE DHT11   // DHT 11 
#define DHTTYPE DHT22   // DHT 22  (AM2302)
//#define DHTTYPE DHT21   // DHT 21 (AM2301)
DHT dht(DHTPIN, DHTTYPE)*/
/*
SHResponse CmdHelloHandler(const String command, String* args) {
	if (command == "hello") {
		return SHResponse(true, "This command Hello!!! :-)");
	}
	if (command == "temp" || command == "temperature") {
		//float h = dht.readHumidity();
		//float t = dht.readTemperature();
		float h = 53.69;
		float t = 27.56;

		JsonClass json = JsonClass();
		json.beginObject();
		json.addProperty("humidity");
		json.addPropertyValue(h);
		json.addProperty("temperature");
		json.addPropertyValue(t);
		json.endObject();

		Serial.println(json.toString());
		return SHResponse(true, json.toString());
	}
	return SHResponse(false, "Command not found!");
}*/

// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(115200);
	Serial.setDebugOutput(true);
	Serial.println("worked!");

	SHWebSocket.beginWifi("ORC", "Pass@orc");
	SHWebSocket.beginWebSocket("89.218.189.54", 80, "/ws", "arduino", "mk", "sss");


	//SHWebServer.beginWifi("mknet", "rty456FGH$%^vbn");
	//SHWebServer.beginServer(80, CmdHelloHandler);


	//dht.begin();

	//CommandLine.serialBegin(115200);
}

// the loop function runs over and over again until power down or reset
void loop() {
  //CommandLine.start(CommandHandler);
	//SHWebServer.handleClient();
	SHWebSocket.webSocketLoop();
}
