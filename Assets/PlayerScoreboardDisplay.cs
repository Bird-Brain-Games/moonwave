using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScoreboardDisplay : MonoBehaviour {

	public PlayerScoreboardSection sectionPrefab;
	PlayerManager playerManager;
	PlayerScoreboardSection[] sections;
	ScoreDisplay scoreDisplay;
	int numPlayers;


	// Use this for initialization
	void Start () {
		playerManager = (PlayerManager)FindObjectOfType(typeof(PlayerManager));
		scoreDisplay = (ScoreDisplay)FindObjectOfType(typeof(ScoreDisplay));
		numPlayers = playerManager.GetNumPlayers();
		sections = new PlayerScoreboardSection[numPlayers];

		for (int i = 0; i < numPlayers; i++)
		{
			sections[i] = Instantiate(sectionPrefab, transform);
			sections[i].image.color = playerManager.players[i].colour;

		}

		UpdateAllScores();
	}

	public void UpdateAllScores()
	{
		// If we're running it inside the editor and not from the menu setup,
		// Initialize the settings
		if (Application.isEditor && MatchSettings.numPlayers == 0)
        {
			MatchSettings.numPlayers = numPlayers;
			MatchSettings.pointsToWin = 1;

			for(int i = 0; i < numPlayers; i++)
			{
				MatchSettings.playerScores.Add(0);
				MatchSettings.playerColors.Add(sections[i].image.color);
			}
		}

		for(int i = 0; i < numPlayers; i++)
		{
			UpdateScores(i);
		}
	}

	// Increase the score of a player by 1.
	// Using the player num from 0-3 [Graham]
	void UpdateScores(int playerNum)
	{
		// Safety, can't exceed array
		if (playerNum > numPlayers) return;

		if (scoreDisplay.stockMode && scoreDisplay.pointPerKill)
			sections[playerNum].Score += MatchSettings.playerScores[playerNum] + playerManager.players[playerNum].getScore();
		else if (scoreDisplay.stockMode)
			sections[playerNum].Score += MatchSettings.playerScores[playerNum] + ((playerManager.playerLives[playerNum] >= 0) ? 1 : 0);

	}
}
