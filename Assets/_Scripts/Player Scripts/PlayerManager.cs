using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public Unique playerPrefab; 
    public Portrait portraitPrefab;
    public PortraitManager portraitManager;
    public LevelSelectManager levelSelectManager;
    public PlayerStats[] players;
    public int[] playerScores;
    public Color[] playerColours;
    public int[] playerLives;
    int numPlayers;
    int colorDirection;
    int mapSelectDirection;
    int mapID;
    public bool selectScreen;
    public bool mapSelect;
    Vector3 outOfBounds;
    Vector3 spawnIn;

    Controls[] controls;
    Spawn spawn;

    void CreatePlayers()
    {
        // Create our players
        numPlayers = MatchSettings.numPlayers;
        if (numPlayers == 0)    // Debug 
            numPlayers = 4;

        players = new PlayerStats[numPlayers];
        controls = new Controls[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            Unique temp = Instantiate(playerPrefab, transform);
            if (temp == null)
            {
                Debug.LogError("Error creating player");
            }

            players[i] = temp.GetComponentInChildren<PlayerStats>();
            controls[i] = temp.GetComponentInChildren<Controls>();
        }

        
    }

    void SetUpPlayers()
    {
        // Get the player colours [Graham]
        for (int i = 0; i < numPlayers; i++)
        {
            if (MatchSettings.numPlayers > 0)
            {
                players[i].colour = MatchSettings.playerColors[i];
            }
            //Debug.Log("Initializing colours");
            playerColours[i] = players[i].colour;
        }

        // Set the player IDs, for scoring [Jack]
        for (int i = 0; i < numPlayers; i++)
        {
            // i + 1 to make Player1 actually Player1, etc
            players[i].m_PlayerID = i;
        }

        // Set the players to be a million lightyears away
        if (selectScreen)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                players[i].transform.position = outOfBounds;
                //Portrait temp = Instantiate(portraitPrefab, players[i].transform.parent);
                //temp.transform.Translate(new Vector3(i * 50f, 0f, 0f));
            }
        }
        else
        {
            // Reset the players
            for (int i = 0; i < numPlayers; i++)
            {
                players[i].transform.position = spawn.getInitialSpawn(i);
            }
        }
    }
    
	// Use this for initialization
    void Awake()
    {
        spawn = GetComponent<Spawn>();
        CreatePlayers();
        
        playerScores = new int[numPlayers];
        playerColours = new Color[numPlayers];
        playerLives = new int[numPlayers];
        outOfBounds = new Vector3(300.0f, 300.0f, 300.0f);
        spawnIn = new Vector3(-52, 0.5f, 0);
    }

	void Start () {
        SetUpPlayers();

        
    }

    // Update is called once per frame
    void Update () {
        // Update the score counters and life counters [Graham][Jack]
        for (int i = 0; i < numPlayers; i++)
        {
            playerScores[i] = players[i].getScore();
            playerLives[i] = players[i].getLives();
        }

        if (selectScreen)   characterLobby();
        else if (mapSelect) mapSelectLobby();
	}



    public void SwitchLobbies()
    {
        if (selectScreen)
        {
            selectScreen = false;
            mapSelect = true;
            
            // Blast the players out to space so they don't interfere [Graham]
            for (int i = 0; i < numPlayers; i++)
                players[i].transform.position = outOfBounds;
        }
        else if (mapSelect)
        {
            selectScreen = true;
            mapSelect = false;

            // If there are active players, bring them back
            for (int i = 0; i < numPlayers; i++)
            {
                players[i].playerSelecting = false;
                players[i].playerConfirmed = false;
            } 
        }
    }

    void mapSelectLobby()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            if (players[i].playerConfirmed)
            {
                // Handle inputs
                mapSelectDirection = controls[i].GetColorChange();

                // Handle inputs
                if (mapSelectDirection == 1)
                    levelSelectManager.NextImage(i);
                else if (mapSelectDirection == -1)
                    levelSelectManager.PrevImage(i);
                if (controls[i].GetDeselect())
                {
                    if (levelSelectManager.GetReady(i))
                        levelSelectManager.SetReady(i, false);
                    else
                    {
                        levelSelectManager.GoBack();
                        SwitchLobbies();
                    }
                }
                else if (controls[i].GetSelect())
                {
                    levelSelectManager.SetReady(i, true);
                }
            }
        }
    }

    void characterLobby()        // Character color selection lobby [Jack]
    {
        for (int i = 0; i < numPlayers; i++)
        {

            if (portraitManager.IsReady(i))    // If the player is ready [Graham]
            {
                // Handle inputs
                if (controls[i].GetDeselect())
                {
                    portraitManager.SetReady(i, false);
                    players[i].transform.position = outOfBounds;
                    players[i].playerSelecting = true;
                    players[i].playerConfirmed = false;
                }
                else if (controls[i].GetStart())
                {
                    StartMatch();
                }
            }
            else if (portraitManager.IsActive(i))        // If the player is active [Graham]
            {
                colorDirection = controls[i].GetColorChange();

                // Handle inputs
                if (colorDirection == 1)
                    portraitManager.NextImage(i);
                else if (colorDirection == -1)
                    portraitManager.PrevImage(i);
                else if (controls[i].GetDeselect())
                {
                    portraitManager.SetActive(i, false);
                    players[i].playerSelecting = false;
                    players[i].playerConfirmed = false;
                }
                else if (controls[i].GetSelect())
                {
                    portraitManager.SetReady(i, true);
                    players[i].setColour(portraitManager.GetColor(i));
                    players[i].setBulletParticleColour(i);

                    players[i].transform.position = players[i].defaultSpawn;
                    players[i].GetComponent<Rigidbody>().ResetInertiaTensor();
                    players[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                    players[i].playerSelecting = false;
                    players[i].playerConfirmed = true;

                }
            }
            else                                    // If the player is neither active nor ready
            {
                if (controls[i].GetSelect())
                {
                    portraitManager.SetActive(i, true);
                    players[i].playerSelecting = true;
                    players[i].playerConfirmed = false;
                }
            }
            
        }
    }

    void StartMatch()
	{
		int numReadyPlayers = 0;
		foreach (PlayerStats stats in players)
		{
			if (stats.playerConfirmed)
				numReadyPlayers++;
		}

		if (numReadyPlayers > 1)	// To be changed [Graham]
		{
			//MatchSettings.numPlayers = 4;	// TO BE CHANGED
			MatchSettings.numPlayers = numReadyPlayers;	
			MatchSettings.pointsToWin = 3;

			for (int i = 0; i < MatchSettings.numPlayers; i++)
			{
				MatchSettings.playerColors.Add(players[i].colour);
				MatchSettings.playerScores.Add(0);
			}

			// To be changed [Graham]
			//GetComponent<LoadSceneOnClick>().LoadRandom();
			//GetComponent<LoadSceneOnClick>().LoadByName("_Scenes/Debug/Graham_Debug");
		
			SwitchLobbies();
            FindObjectOfType<LobbyManager>().ShowLevelSelect();
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
