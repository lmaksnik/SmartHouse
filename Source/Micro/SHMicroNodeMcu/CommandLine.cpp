// 
// 
// 

#include "CommandLine.h"
#include <vector>

void CommandLineClass::init()
{
}

void CommandLineClass::serialBegin(const unsigned long baud)
{
	USE_SERIAL.begin(baud);
}

void CommandLineClass::start(TCmdHandlerFunction handler)
{
	this->_handler = handler;
	USE_SERIAL.println("");
	USE_SERIAL.println("CMD enabled!");

	USE_SERIAL.print("cmd>");
	while (true) {
		String line = USE_SERIAL.readString();
		if (line != nullptr && line.length() > 0)
		{
			USE_SERIAL.println(line);
			callCommand(line);

			USE_SERIAL.println("");
			USE_SERIAL.print("cmd>");
		}
	}
}

void CommandLineClass::callCommand(String str) const
{
	int idx = str.indexOf(' ');
	if (idx < 0) {
		callCommand(str, {});
		return;
	}

	const String cmdName = str.substring(0, idx);
	str.remove(0, idx + 1);
	
	std::vector<String> tokens;
	while ((idx = str.indexOf(' ')) > -1)
	{
		tokens.push_back(str.substring(0, idx));
		str.remove(0, idx + 1);
	}
	if (str != nullptr && str.length() > 0)
		tokens.push_back(str);

	callCommand(cmdName, tokens.data());
}

void CommandLineClass::callCommand(const String command, String* parameters) const
{
	if (_handler == nullptr)
	{
		USE_SERIAL.println("Command handler not found!");
	}
	else
	{
		_handler(command, parameters);
	}
}


CommandLineClass CommandLine;

