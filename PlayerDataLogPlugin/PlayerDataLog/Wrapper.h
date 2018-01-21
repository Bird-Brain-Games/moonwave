//////////////////////////////////////////////////////////////////////////////////
//   Robert Savaglio - 100591436	    and      Jack Hamilton   - 100550931	//
//																				//
//  Code taken from Lab 2 and expanded for use as a player data log in our game //
//////////////////////////////////////////////////////////////////////////////////

#pragma once
#include "LibSettings.h"

#ifdef __cplusplus
extern "C"
{
#endif
	// Entry Points
	LIB_API void Log(char *a_Directory, char *a_fileName, char *a_playerNum, char *a_hangTime, char *a_groundTime, char *a_bullets, char *a_boosts, char *a_deaths);
	LIB_API void LogHeader(char *a_Directory, char *a_fileName, char *a_dateAndTime);
#ifdef __cplusplus
}
#endif