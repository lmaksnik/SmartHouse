// cmd.h

#ifndef _CMD_h
#define _CMD_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#define USE_SERIAL Serial
#include <functional>

class Cmd
{
public:
	typedef std::function<void(String, String*)> TCmdHandlerFunction;
	void Start(unsigned long baud, TCmdHandlerFunction handler);
private:
	void callCommand(String str);
	void callCommand(String command, String* parameters);

	TCmdHandlerFunction _handler;
};


#endif

