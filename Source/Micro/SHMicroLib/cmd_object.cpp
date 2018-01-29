// 
// 
// 

#include "cmd_object.h"


String CmdObject::get_cmd_name()
{
	return this->cmd_name;
}

String* CmdObject::get_cmd_parameters()
{
	return this->cmd_parameters;
}

bool CmdObject::is_empty()
{
	return this->is_empty();
}
