// 
// 
// 

#include "cmd.h"


void Cmd::Start(unsigned long baud, TCmdHandlerFunction handler)
{
	this->_handler = handler;
	USE_SERIAL.begin(baud);
	USE_SERIAL.println("");
	USE_SERIAL.println("CMD enabled!");
	//try {
		while(true) {
			USE_SERIAL.print("cmd>");
			String line = USE_SERIAL.readString();
			if (line != nullptr && line.length() > 0)
			{
				USE_SERIAL.println("");
				callCommand(line);
				/*CmdObject cmd = parseStr(line);
				if (cmd.is_empty())
				{
					USE_SERIAL.println("Incorrect command!");
				} else
				{
					callCmd(cmd);
				}*/
			}
		}
	/*} catch(const std::exception& e)
	{
		USE_SERIAL.println("");
		USE_SERIAL.println("CMD ERROR!");
		USE_SERIAL.println(e.what());
	}*/
}

void Cmd::callCommand(String str)
{

	String* params = {};
	int idx = str.indexOf(' ');
	if (idx < 0) {
		callCommand(str, params);
		return;
	}
	String cmdName = str.substring(0, idx);
	str.remove(0, idx + 1);

	int i = 0;
	while ((idx = str.indexOf(' ')) > -1)
	{
		params[i] = str.substring(0, idx);
		str.remove(0, idx + 1);
		i++;
	}
	callCommand(cmdName, params);
}

void Cmd::callCommand(String command, String* parameters)
{
	if (_handler == nullptr)
	{
		USE_SERIAL.println("Command handler not found!");
	}
	else
	{
		//try {
		_handler(command, parameters);
		/*} catch(const std::exception& e)
		{
		USE_SERIAL.print("Command \"");
		USE_SERIAL.print(cmd.get_cmd_name());
		USE_SERIAL.println("\" error!");
		USE_SERIAL.println(e.what());
		}*/
	}
}
//
//CmdObject Cmd::parseStr(String str)
//{
//	int idx= str.indexOf(' ');
//	if (idx < 0)
//	{
//		return CmdObject(str, new const String);
//	}
//	String cmdName = str.substring(0, idx);
//	str.remove(0, idx + 1);
//
//	String* params = {};
//	int i = 0;
//	while ((idx = str.indexOf(' ')) > -1)
//	{
//		params[i] = str.substring(0, idx);
//		str.remove(0, idx + 1);
//		i++;
//	}
//	return CmdObject(cmdName, params);
//}
//
//void Cmd::callCmd(CmdObject cmd)
//{
//	if (_handler == nullptr)
//	{
//		USE_SERIAL.println("Command handler not found!");
//	}
//	else
//	{
//		//try {
//		//_handler(cmd);
//		/*} catch(const std::exception& e)
//		{
//			USE_SERIAL.print("Command \"");
//			USE_SERIAL.print(cmd.get_cmd_name());
//			USE_SERIAL.println("\" error!");
//			USE_SERIAL.println(e.what());
//		}*/
//	}
//}
