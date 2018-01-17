//////////////////////////////////////////////////////////////////////////////////
//   Robert Savaglio - 100591436	    and      Jack Hamilton   - 100550931	//
//																				//
//  Code taken from Lab 2 and expanded for use as a player data log in our game //
//////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class LogOutput : MonoBehaviour
{
    // Entry Points from the PlayerDataLog DLL
    [DllImport("PlayerDataLog")]
    public static extern IntPtr LogHeader(string fileDirectory, string fileName, string dateAndTime);

    [DllImport("PlayerDataLog")]
    public static extern void Log(string fileDirectory, string fileName, string playerNum, string hangTime, string groundTime, string bullets, string boosts, string deaths);


    // On application quit the player data log is updated to include the new player info from the play session
    void OnDestroy()
    {
        String dateAndTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

        LogHeader(transform.name, "PlayerDataLog", dateAndTime); // Creates a new header in the file with the date and time

        for (int i = 4; i < transform.childCount; i++) // Writes the data for each player to the file
        {
            Log(transform.name, "PlayerDataLog",
                (i - 3).ToString(),

                transform.GetChild(i).GetChild(0).GetComponent<StickToPlanet>().l_hangTime.ToString(),
                transform.GetChild(i).GetChild(0).GetComponent<StickToPlanet>().l_groundTime.ToString(),
                transform.GetChild(i).GetChild(0).GetComponent<Shoot>().l_bullets.ToString(),
                transform.GetChild(i).GetChild(0).GetComponent<PlayerBoost>().l_boosts.ToString(),
                transform.GetChild(i).GetChild(0).GetComponent<KnockOut>().l_deaths.ToString()
                );
        }
    }

}