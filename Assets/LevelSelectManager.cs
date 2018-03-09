using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelSelectManager : MonoBehaviour {

	public UnityEvent goBack;
	public PlayerManager playerManager;
	public Image chosenStageImage;
	public Sprite[] choosingImages;

	public Sprite[] readyImages;

	bool[] playerReady;
	int[] playerSelection;

	Image[] levelImages;
	int numPlayers;
	bool allReady;

	// Use this for initialization
	void Awake () {
		Reset();
		chosenStageImage.gameObject.SetActive(false);
	}

	public void Reset()
	{
		numPlayers = MatchSettings.numPlayers;
		playerReady = new bool[numPlayers];
		playerSelection = new int[numPlayers];
		levelImages = GetComponentsInChildren<Image>();
		allReady = false;

		// Only enable the images for the active players
		foreach(Image image in levelImages) image.gameObject.SetActive(false);
		for (int i = 0; i < numPlayers; i++)
			levelImages[i].gameObject.SetActive(true);

	}
	
	public void NextImage(int playerNum)
	{
		if (playerReady[playerNum]) return;

		// Increment the selecting index
		playerSelection[playerNum]++;

		// If outside the bounds of the index, go back to the start
		if (playerSelection[playerNum] >= choosingImages.Length)
			playerSelection[playerNum] = 0;


		// Set the portrait sprite
		levelImages[playerNum].sprite = choosingImages[playerSelection[playerNum]];
	}

	public void PrevImage(int playerNum)
	{
		if (playerReady[playerNum]) return;

		// Decrement the selecting index
		playerSelection[playerNum]--;

		// If outside the bounds of the index, go back to the end
		if (playerSelection[playerNum] < 0)
			playerSelection[playerNum] = choosingImages.Length - 1;

		// Set the portrait sprite
		levelImages[playerNum].sprite = choosingImages[playerSelection[playerNum]];
	}

	public void SetReady(int playerNum, bool isReady)
	{
		if (playerReady[playerNum] == isReady || allReady) return;	// If the same, do nothing [Graham]
		playerReady[playerNum] = isReady;

		// Set the portrait image
		if (isReady)
		{
			levelImages[playerNum].sprite = readyImages[playerSelection[playerNum]];	
		}
		else
			levelImages[playerNum].sprite = choosingImages[playerSelection[playerNum]];

		// Check if all players have chosen
		allReady = true;
		for (int i = 0; i < numPlayers; i++)
		{
			if (!playerReady[i]) allReady = false;
		}

		if (allReady) StartGame();

	}

	public bool GetReady(int playerNum)
	{
		if (playerNum > numPlayers) return false;
		return playerReady[playerNum];
	}

	public void GoBack()
	{
		if (allReady) return;
		goBack.Invoke();
	}

	void StartGame()
	{
		IEnumerator randomStageSelect = RandomStageSelect(3,5);
		chosenStageImage.gameObject.SetActive(true);
		StartCoroutine(randomStageSelect);
	}

	IEnumerator RandomStageSelect(int minTime, int maxTime)
	{
		float countdownTime = Random.Range((float)minTime, (float)maxTime);	// Not currently used
		float delay = 0f;
		int increment = 0;

		// Main loop
		while (delay < 0.5)
		{
			if (delay < 0.2f)
				delay += 0.01f;
			else
				delay += 0.05f;

			increment++;
			if (increment >= numPlayers) increment = 0;
			chosenStageImage.sprite = readyImages[increment];
			yield return new WaitForSeconds(delay);
		}
		
		// Start using the chosen stage selections
		yield return new WaitForSeconds(2f);

		// TO BE CHANGED [Graham]
		//GetComponent<LoadSceneOnClick>().LoadRandom();
		GetComponent<LoadSceneOnClick>().LoadBySet(playerSelection[increment]);
	}

}
