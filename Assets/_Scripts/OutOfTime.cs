using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfTime : MonoBehaviour {

    public GameObject timerObject;
    public GameObject backToMenuButton;
    public GameObject scoreBoard;
    ScoreDisplay m_scoringType;
    Text m_Text;
    Timer timer;
    Animator m_Animator;


	// Use this for initialization
	void Awake () {
        timer = timerObject.GetComponent<Timer>();
        m_Animator = GetComponent<Animator>();
        m_scoringType = GameObject.Find("Player Score Panel").GetComponent<ScoreDisplay>();
	}
	
	// Update is called once per frame
	void Update () {

        // Update the animator time control (for transitions) [Graham]
        if (!m_scoringType.stockMode) // If stock is turned off, turn on timer [Jack]
        {
            m_Animator.SetFloat("time", timer.GetTime());

            if (m_Animator.GetBool("InGame"))
            {
                // Hacky, but it works for now [Graham]
                timer.Show();
            }
        }
            
        //If stock mode is turned ON, check to see if there is one player left for the win screen [Jack]
        else if (m_scoringType.playersInGame == 1)
        {
            EndMatch();
        }


        if (m_Animator.GetBool("Results Showing"))  // I hate that this is polling every frame, but it has to in this system [Graham]
        {
            if (!scoreBoard) Debug.LogError("Scoreboard not hooked up to Match Text yet (yuck)");
            if (!scoreBoard.activeInHierarchy)
            {
                scoreBoard.SetActive(true);
                PlayerScoreboardDisplay display = scoreBoard.GetComponentInChildren<PlayerScoreboardDisplay>();
                //display.UpdateAllScores();
            }
            
        }
    }

    public void EndMatch()
    {
        // If the match is finished, tell the animator [Graham]
        timer.Hide();
        m_Animator.SetTrigger("End Match");

        // If the results are showing, tell the back to menu button to enable itself
        // Shouldn't be in this file, but because it's locked to the animation, 
        // It kinda has to go here. Sorry! [Graham]

        // if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Show Results") && // Somewhat unstable [Graham]
        //     !backToMenuButton.activeInHierarchy)   
        //     backToMenuButton.SetActive(true);
    
    }
}
