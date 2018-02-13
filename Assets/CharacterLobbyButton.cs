using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLobbyButton : MonoBehaviour {

	public List<PlayerStats> players;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("START"))
			StartMatch();
	}

	void StartMatch()
	{
		int numReadyPlayers = 0;
		foreach (PlayerStats stats in players)
		{
			if (stats.playerConfirmed)
				numReadyPlayers++;
		}

		if (numReadyPlayers > 0)	// To be changed [Graham]
		{
			MatchSettings.numPlayers = numReadyPlayers;
			MatchSettings.pointsToWin = 3;

			foreach(PlayerStats stats in players)
			{
				MatchSettings.playerColors.Add(stats.colour);
			}

			// To be changed [Graham]
			GetComponent<LoadSceneOnClick>().LoadRandom();
		}
	}
}
