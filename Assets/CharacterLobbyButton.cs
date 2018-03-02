using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLobbyButton : MonoBehaviour {

	public PlayerManager playerManager;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("START"))
			StartMatch();
	}

	void StartMatch()
	{
		int numReadyPlayers = 0;
		foreach (PlayerStats stats in playerManager.players)
		{
			if (stats.playerConfirmed)
				numReadyPlayers++;
		}

		if (numReadyPlayers > 0)	// To be changed [Graham]
		{
			//MatchSettings.numPlayers = 4;	// TO BE CHANGED
			MatchSettings.numPlayers = numReadyPlayers;	
			MatchSettings.pointsToWin = 3;

			for (int i = 0; i < MatchSettings.numPlayers; i++)
			{
				MatchSettings.playerColors.Add(playerManager.players[i].colour);
				MatchSettings.playerScores.Add(0);
			}

			// To be changed [Graham]
			GetComponent<LoadSceneOnClick>().LoadRandom();
			//GetComponent<LoadSceneOnClick>().LoadByName("_Scenes/Debug/Graham_Debug");
		}
	}
}
