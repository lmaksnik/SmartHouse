// cmd_object.h

#ifndef _CMD_OBJECT_h
#define _CMD_OBJECT_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class CmdObject
{
public:
	CmdObject(const String& cmd_name, const String* cmd_parameters)
		: cmd_name(cmd_name), cmd_is_empty(false)
	{
		cmd_parameters = cmd_parameters;
	}

	CmdObject()
		: cmd_is_empty(true)
	{
	}


	String get_cmd_name();
	String* get_cmd_parameters();
	bool is_empty();
private:
	String cmd_name;
	bool cmd_is_empty;
	String cmd_parameters[];
};

#endif

