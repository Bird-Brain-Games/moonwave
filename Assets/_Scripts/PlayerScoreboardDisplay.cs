using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScoreboardDisplay : MonoBehaviour {

	public PlayerScoreboardSection sectionPrefab;
	public Image winImage;
	public Text winText;

	public GameObject scoreTitle;
    public GameObject nextMapButton;
	public GameObject toMainMenuButton;
	public GameObject continueText;

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

		// Debug, empty initialization [Graham]
		if (MatchSettings.numPlayers == 0)
		{
			for (int i = 0; i < numPlayers; i++)
            {
                MatchSettings.playerImages.Add(new Sprite());
                MatchSettings.playerReadyImages.Add(new Sprite());
            }
		}

		for (int i = 0; i < numPlayers; i++)
		{
			sections[i] = Instantiate(sectionPrefab, transform);
			sections[i].image.sprite = MatchSettings.playerImages[i];
			sections[i].color = playerManager.players[i].colour;
			sections[i].doneAnimating.AddListener(delegate{FinishScoreboard();});

			// Show the color if in debug [Graham]
			if (MatchSettings.numPlayers == 0) 
				sections[i].image.color = playerManager.players[i].colour;
		}

		UpdateAllScores();
	}

	void FinishScoreboard()
	{
		nextMapButton.SetActive(true);
		continueText.SetActive(true);

		// Check for a winner
		for(int i = 0; i < numPlayers; i++)
		{
			if(MatchSettings.playerScores[i] >= MatchSettings.pointsToWin)
			{
				ShowWinner(i);
			}
		}
	}

	public void UpdateAllScores()
	{
		// If we're running it inside the editor and not from the menu setup,
		// Initialize the settings
		if (Application.isEditor && MatchSettings.numPlayers == 0)
        {
			MatchSettings.numPlayers = numPlayers;
			MatchSettings.pointsToWin = 3;

			for(int i = 0; i < numPlayers; i++)
			{
				MatchSettings.playerScores.Add(0);
				MatchSettings.playerColors.Add(sections[i].image.color);
			}
		}

		for(int i = 0; i < numPlayers; i++)
		{
			sections[i].Score = MatchSettings.playerScores[i];
			sections[i].Populate();
			UpdateScores(i);
		}

		// Set the portrait of the person in the lead
		int highestIndex = GetHighestScoreIndex();
		if (highestIndex >= 0)
			sections[highestIndex].image.sprite = MatchSettings.playerReadyImages[highestIndex];
	}

	// Increase the score of a player by 1.
	// Using the player num from 0-3 [Graham]
	void UpdateScores(int playerNum)
	{
		// Safety, can't exceed array
		if (playerNum > numPlayers) return;

		int oldScore = MatchSettings.playerScores[playerNum];

		// Choose how many points to add
		if (scoreDisplay.stockMode && scoreDisplay.pointPerKill)
			MatchSettings.playerScores[playerNum] += playerManager.players[playerNum].getScore();
		else if (scoreDisplay.stockMode)
			MatchSettings.playerScores[playerNum] += ((playerManager.playerLives[playerNum] >= 0) ? 1 : 0);
		
		sections[playerNum].Score = MatchSettings.playerScores[playerNum];
		if (oldScore < sections[playerNum].Score)
			sections[playerNum].AnimateLastLight();
	}

	void ShowWinner(int playerNum)
	{
		scoreTitle.gameObject.SetActive(false);
		winImage.gameObject.SetActive(true);
		winImage.sprite = MatchSettings.playerReadyImages[playerNum];
		winText.gameObject.SetActive(true);
		//winImage.color = MatchSettings.playerColors[playerNum];

		// Disable all the player sections so you can't see the score (possible to change) [Graham]
		foreach (var section in sections)
		{
			section.gameObject.SetActive(false);
		}

		nextMapButton.SetActive(false);
		toMainMenuButton.SetActive(true);
	}

	int GetHighestScoreIndex()
	{
		int index = 0;
		int highestScore = -1;
		for (int i = 0; i < numPlayers; i++)
		{
			if (sections[i].Score > highestScore)
			{
				highestScore = sections[i].Score;
				index = i;
			}
			else if (sections[i].Score == highestScore)
				return -1;
		}

		return index;
	}
}
