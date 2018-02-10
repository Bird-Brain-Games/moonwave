using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public PlayerStats[] players;
    public int[] playerScores;
    public Color[] playerColours;
    public int[] playerLives;
    int numPlayers;
    int colorDirection;
    public bool selectScreen;

    Controls controls;
    
	// Use this for initialization
	void Awake () {
        players = GetComponentsInChildren<PlayerStats>();
        numPlayers = players.Length;
        playerScores = new int[numPlayers];
        playerColours = new Color[numPlayers];
        playerLives = new int[numPlayers];

        // Get the player colours [Graham]
        for (int i = 0; i < numPlayers; i++)
        {
            //Debug.Log("Initializing colours");
            playerColours[i] = players[i].colour;
        }

        // Set the player IDs, for scoring [Jack]
        for (int i = 0; i < numPlayers; i++)
        {
            // i + 1 to make Player1 actually Player1, etc
            players[i].m_PlayerID = i;
        }

    }

    // Update is called once per frame
    void Update () {
        // Update the score counters with the player scores [Graham]
        for (int i = 0; i < numPlayers; i++)
        {
            playerScores[i] = players[i].getScore();
        }

        // Update the player Lives [Jack]
        for (int i = 0; i < numPlayers; i++)
        {
            playerLives[i] = players[i].getLives();
        }

        // Character color lobby [Jack]
        if(selectScreen)
        {
            GetComponentInParent<bulletColour>().freeColors();

            for (int i = 0; i < numPlayers; i++)
            {
                colorDirection = players[i].GetComponent<Controls>().GetColorChange();
                //Debug.Log("Initializing colours");

                // Color select to the right
                if (colorDirection == 1)
                {
                    playerColours[i] = players[i].selectColourRight();
                    Debug.Log("changing colour!");
                }

                // Color select to the left
                if (colorDirection == -1)
                {
                    playerColours[i] = players[i].selectColourLeft();
                    Debug.Log("changing colour left!");
                }

                // Confirm color selection
                if (players[i].GetComponent<Controls>().GetSelect())
                {
                    players[i].confirmColor();
                }

            }


        }
	}

    /// <summary>
    /// Get the number of players [Graham]
    /// </summary>
    /// <returns></returns>
    public int GetNumPlayers()
    {
        return numPlayers;
    }

    /// <summary>
    /// Get the array of player scores [Graham]
    /// </summary>
    /// <returns></returns>
    public int[] GetPlayerScores()
    {
        return playerScores;
    }

    /// <summary>
    /// Get the array of player lives
    /// </summary>
    /// <returns></returns>
    public int[] GetPlayerLives()
    {
        return playerLives;
    }

    /// <summary>
    /// Get the array of player colours [Graham]
    /// </summary>
    /// <returns></returns>
    public Color[] GetPlayerColours()
    {
        return playerColours;
    }

    public void roundReset()
    {
        PlayerStateManager[] list;
        list = GetComponentsInChildren<PlayerStateManager>();
        foreach(PlayerStateManager element in list)
        {
            element.resetRound();
        }
    }
}
