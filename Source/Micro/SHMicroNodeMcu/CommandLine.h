
// CommandLine.h

#ifndef _COMMANDLINE_h
#define _COMMANDLINE_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#define USE_SERIAL Serial
#include <functional>

class CommandLineClass
{
 protected:


 public:
	void init();
	void serialBegin(unsigned long baud);

	typedef std::function<void(String, String*)> TCmdHandlerFunction;
	void start(TCmdHandlerFunction handler);

private:
	void callCommand(String str) const;
	void callCommand(String command, String* parameters) const;

	TCmdHandlerFunction _handler;
};

extern CommandLineClass CommandLine;

#endif

