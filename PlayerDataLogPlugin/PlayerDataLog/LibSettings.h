//////////////////////////////////////////////////////////////////////////////////
//   Robert Savaglio - 100591436	    and      Jack Hamilton   - 100550931	//
//																				//
//  Code taken from Lab 2 and expanded for use as a player data log in our game //
//////////////////////////////////////////////////////////////////////////////////

#pragma once
#define PLAYERDATALOG_EXPORTS

#ifdef PLAYERDATALOG_EXPORTS
#define LIB_API __declspec(dllexport)
#elif PLAYERDATALOG_EXPORTS
#define LIB_API __declspec(dllimport)
#else
#define LIB_API
#endif