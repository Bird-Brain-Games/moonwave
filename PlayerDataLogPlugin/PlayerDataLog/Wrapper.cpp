//////////////////////////////////////////////////////////////////////////////////
//   Robert Savaglio - 100591436	    and      Jack Hamilton   - 100550931	//
//																				//
//  Code taken from Lab 2 and expanded for use as a player data log in our game //
//////////////////////////////////////////////////////////////////////////////////

#include "Wrapper.h"
#include "windows.h"
#include <string>
#include <fstream>
#include <iostream>


void LogHeader(char *a_Directory, char *a_fileName, char *a_dateAndTime)
{
	// Opens the directory and outputs an error message if it fails
	CreateDirectoryA(a_Directory, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND)
	{
		std::string l_ErrorMsg = "Error creating directory: ";
		l_ErrorMsg += a_Directory;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);
		Out << l_ErrorMsg;
		Out.close();
		return;
	}

	// Create the file name for the log
	std::string l_Dir(a_Directory);
	l_Dir += "/";
	l_Dir += a_fileName;
	l_Dir += ".txt";

	// Open the log for writing the header
	std::ofstream l_Out;
	l_Out.open(l_Dir, std::ios_base::app);

	// Lots of starts for clean reading and aesthetics
	l_Out << std::endl << "*******************************" << std::endl;
	l_Out << "***** " << a_dateAndTime << " *****" << std::endl;
	l_Out << "*******************************" << std::endl;
	l_Out << std::endl;
}

void Log(char *a_Directory, char *a_fileName, char *a_playerNum, char *a_hangTime, char *a_groundTime, char *a_bullets, char *a_boosts, char *a_deaths)
{

	// Opens the directory and outputs an error message if it fails
	CreateDirectoryA(a_Directory, NULL);
	if (GetLastError() == ERROR_PATH_NOT_FOUND)
	{
		std::string l_ErrorMsg = "Error creating directory: ";
		l_ErrorMsg += a_Directory;
		std::string ErrorFile = "ErrorOutput.txt";
		std::ofstream Out(ErrorFile);
		Out << l_ErrorMsg;
		Out.close();
		return;
	}

	// Create the file name for the log
	std::string l_Dir(a_Directory);
	l_Dir += "/";
	l_Dir += a_fileName;
	l_Dir += ".txt";

	// Open the log for writing
	std::ofstream l_Out;
	l_Out.open(l_Dir, std::ios_base::app);

	// Output the log info for the player to the file
	l_Out << "**** PLAYER " << a_playerNum << " ****" << std::endl;
	l_Out << "Total Hang Time: " << a_hangTime << std::endl;
	l_Out << "Total Time Grounded: " << a_groundTime << std::endl;
	l_Out << "Total Bullets Shot: " << a_bullets << std::endl;
	l_Out << "Total Boosts Used: " << a_boosts << std::endl;
	l_Out << "Total Deaths: " << a_deaths << std::endl;

	l_Out << std::endl;

	l_Out.close();
}