using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour {

	// Portrait resources
	public Sprite[] choosingImages;
	public Sprite[] readyImages;

	// Player related variables
	bool[] playerReady;
	bool[] playerActive;
	int[] playerSelection;
	int numPlayers;

	// Portrait variables
	bool[] available;
	Image[] portraits;
	Color[] colors;

	// Use this for initialization
	void Awake () {
		numPlayers = 4;

		portraits = GetComponentsInChildren<Image>();
		
		playerReady = new bool[numPlayers];
		playerActive = new bool[numPlayers];
		playerSelection = new int[numPlayers];

		SetColors();
		Reset();
	}

	public void Reset()
	{
		// Set all players as inactive
		for(int i = 0; i < numPlayers; i++)
		{
			playerActive[i] = true;	// workaround so we can set it as false
			SetActive(i, false);
			SetReady(i, false);
		}

		// Set all images to available
		available = new bool[choosingImages.Length];
		for (int i = 0; i < available.Length; i++) available[i] = true;
	}

	void SetColors()
	{
		colors = new Color[choosingImages.Length];
		colors[0] = new Color32(79, 187, 255, 255); //green
		colors[1] = new Color32(26, 156, 41, 255); //orange
		colors[2] = new Color32(250, 70, 32, 255); //purple
		colors[3] = new Color32(165, 36, 197, 255); //teal????
		colors[4] = new Color32(115, 117, 128, 255); // light blue 
	}

	public Color GetColor(int playerNum)
	{
		return colors[playerSelection[playerNum]];
	}
	
	public void NextImage(int playerNum)
	{
		if (playerReady[playerNum]) return;

		do
		{
			// Increment the selecting index
			playerSelection[playerNum]++;

			// If outside the bounds of the index, go back to the start
			if (playerSelection[playerNum] >= choosingImages.Length)
				playerSelection[playerNum] = 0;
		} 
		while (!available[playerSelection[playerNum]]);

		// Set the portrait sprite
		portraits[playerNum].sprite = choosingImages[playerSelection[playerNum]];
	}

	public void PrevImage(int playerNum)
	{
		if (playerReady[playerNum]) return;

		do
		{
			// Decrement the selecting index
			playerSelection[playerNum]--;

			// If outside the bounds of the index, go back to the end
			if (playerSelection[playerNum] < 0)
				playerSelection[playerNum] = choosingImages.Length - 1;
		} 
		while (!available[playerSelection[playerNum]]);

		// Set the portrait sprite
		portraits[playerNum].sprite = choosingImages[playerSelection[playerNum]];
	}

	public void SetReady(int playerNum, bool isReady)
	{
		if (playerReady[playerNum] == isReady) return;	// If the same, do nothing [Graham]
		playerReady[playerNum] = isReady;

		available[playerSelection[playerNum]] = !isReady;	// Set if the portrait is available for picking

		// Set the portrait image
		if (isReady)
		{
			portraits[playerNum].sprite = readyImages[playerSelection[playerNum]];	

			// If other unready players are on this portrait, move them.
			for (int i = 0; i < numPlayers; i++)
			{
				if (!playerReady[i] && playerSelection[i] == playerSelection[playerNum])
					NextImage(i);
			}
		}
		else
			portraits[playerNum].sprite = choosingImages[playerSelection[playerNum]];
	}

	public void SetActive(int playerNum, bool isActive)
	{
		// Set the state of the portrait, if the player is active in the scene or not.
		if (playerActive[playerNum] == isActive) return;
		playerActive[playerNum] = isActive;
		portraits[playerNum].enabled = isActive;
		
		if (isActive) NextImage(playerNum);
	}

	public bool IsActive(int playerNum)
	{
		return playerActive[playerNum];
	}

	public bool IsReady(int playerNum)
	{
		return playerReady[playerNum];
	}
}
