using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScoreboardDisplay : MonoBehaviour {

	public PlayerScoreboardSection sectionPrefab;
	public PlayerManager playerManager;
	int[] playerScores;
	PlayerScoreboardSection[] sections;
	int numPlayers;


	// Use this for initialization
	void Start () {
		numPlayers = playerManager.GetNumPlayers();
		playerScores = new int[numPlayers];
		sections = new PlayerScoreboardSection[numPlayers];

		for (int i = 0; i < numPlayers; i++)
		{
			sections[i] = Instantiate(sectionPrefab, transform);
			sections[i].image.color = playerManager.players[i].colour;

		}
	}

	// Increase the score of a player by 1.
	// Using the player num from 0-3 [Graham]
	void UpdateScores(int playerNum)
	{
		// Safety, can't exceed array
		if (playerNum > numPlayers) return;

		sections[playerNum].score++;
	}
}
