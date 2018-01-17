using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    public Text playerScorePrefab;

    int[] playerScores;
    public Color[] playerColours;
    Text[] playerScoreText;
    int numPlayers;
    PlayerManager manager;
    
    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Players and spawns").GetComponent<PlayerManager>();   // COULD BE RISKY
        numPlayers = manager.GetNumPlayers();
        playerScores = manager.GetPlayerScores();
        playerColours = manager.GetPlayerColours();


        // Initialize the UI elements for displaying the score [Graham]
        playerScoreText = new Text[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            playerScoreText[i] = Instantiate(playerScorePrefab, transform);
            //if (i == 0)
            playerScoreText[i].color = manager.players[i].colour;  // Broken for some reason [Graham]
            //if (i == 1)
            //playerScoreText[i].color = manager.players[1].colour;
            playerScoreText[i].text = playerScores[i].ToString();
            Debug.Log("Initializing colours");
        }
        
	}
	
	// Late update is called once per frame, after the other updates. Used for UI. [Graham]
	void LateUpdate () {
        manager = GameObject.Find("Players and spawns").GetComponent<PlayerManager>();
        playerScores = manager.GetPlayerScores();

        // Update the player texts
        for (int i = 0; i < numPlayers; i++)
        {
            playerScoreText[i].color = manager.players[i].colour;
            playerScoreText[i].text = playerScores[i].ToString();
        }
    }
}
