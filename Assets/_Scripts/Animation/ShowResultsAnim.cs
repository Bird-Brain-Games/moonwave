using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowResultsAnim : StateMachineBehaviour {

	//public UnityEngine.UI.Button backToMenuPrefab;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        showStockResults(animator);
    }

	void showStockResults(Animator animator)
	{
		Text resultText = animator.GetComponent<Text>();
		resultText.text = "";
		animator.SetBool("Results Showing", true);
	}

	void showScoreResults(Animator animator)
	{
		Text resultText = animator.GetComponent<Text>();
        PlayerManager players = GameObject.FindObjectOfType<PlayerManager>();

		// Enable the "back to menu" button
		//UnityEngine.UI.Button backToMenu = Instantiate(backToMenuPrefab, resultText.transform);
        
		int maxPlayerScore = -1;	// Keep track of the highest score so we can find out how many players won [Graham]
		int[] playerScores = players.GetPlayerScores();

		// Find the highest player score [Graham]
		for (int i = 0; i < players.GetNumPlayers(); i++)
		{
			if (playerScores[i] > maxPlayerScore)
				maxPlayerScore = playerScores[i];
		}

		// Find which players have that score [Graham]
		bool[] winningPlayers = new bool[players.GetNumPlayers()];
		for (int i = 0; i < players.GetNumPlayers(); i++)
		{
			if (playerScores[i] == maxPlayerScore)
				winningPlayers[i] = true;
		}

		// Build the results string [Graham]
		string resultString = "";
		for (int i = 0; i < players.GetNumPlayers(); i++)
		{
			if (winningPlayers[i])
			{
				if (resultString.Length > 0)
					resultString += " and ";

				resultString += players.players[i].colour.ToString();

                // // Hard code to put the color names on winning screen. No one answered me in chat so I had to do it like this sorry [Hugo]
                // if (i == 0)
				//     resultString += "red";
                // if (i == 1)
                //     resultString += "purple";
                // if (i == 2)
                //     resultString += "blue";
                // if (i == 3)
                //     resultString += "green";
            }
		}

		// Put "win" or "wins" depending on how many people won [Graham]
		if (resultString.Length > 2)
			resultString += " win";
		else
		{
			int winningNum = int.Parse(resultString[1].ToString());
			resultString += " wins";
			resultText.color = players.players[winningNum - 1].colour;	// Colour is bugged
		}

		// Change the text to show the winner(s) [Graham]	
		resultText.text = resultString;
	}
}
