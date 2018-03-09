using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScoreboardDisplay : MonoBehaviour {

	public PlayerScoreboardSection sectionPrefab;
	public Image winImage;
	public Text winText;
	
    public GameObject nextMapButton;
	public GameObject toMainMenuButton;

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

		nextMapButton.SetActive(true);

		UpdateAllScores();
	}

	public void UpdateAllScores()
	{
		// If we're running it inside the editor and not from the menu setup,
		// Initialize the settings
		if (Application.isEditor && MatchSettings.numPlayers == 0)
        {
			MatchSettings.numPlayers = numPlayers;
			MatchSettings.pointsToWin = 2;

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

		// Check for a winner
		for(int i = 0; i < numPlayers; i++)
		{
			if(MatchSettings.playerScores[i] >= MatchSettings.pointsToWin)
			{
				ShowWinner(i);
			}
		}

		if (winImage.gameObject.activeInHierarchy)
		{
			// We might want to store the variables before we leave
			MatchSettings.Reset();
		}
		
	}

	// Increase the score of a player by 1.
	// Using the player num from 0-3 [Graham]
	void UpdateScores(int playerNum)
	{
		// Safety, can't exceed array
		if (playerNum > numPlayers) return;

		// Choose how many points to add
		if (scoreDisplay.stockMode && scoreDisplay.pointPerKill)
			MatchSettings.playerScores[playerNum] += playerManager.players[playerNum].getScore();
		else if (scoreDisplay.stockMode)
			MatchSettings.playerScores[playerNum] += ((playerManager.playerLives[playerNum] >= 0) ? 1 : 0);

		sections[playerNum].Score = MatchSettings.playerScores[playerNum];
	}

	void ShowWinner(int playerNum)
	{
		winImage.gameObject.SetActive(true);
		winText.gameObject.SetActive(true);
		winImage.color = MatchSettings.playerColors[playerNum];

		// Disable all the player sections so you can't see the score (possible to change) [Graham]
		foreach (var section in sections)
		{
			section.gameObject.SetActive(false);
		}

		nextMapButton.SetActive(false);
		toMainMenuButton.SetActive(true);

		
	}
}
