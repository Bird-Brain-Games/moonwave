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
    PlayerStateManager[] players;
    bool matchActive;


	// Use this for initialization
	void Awake () {
        timer = timerObject.GetComponent<Timer>();
        m_Animator = GetComponent<Animator>();
        m_scoringType = GameObject.Find("Player Score Panel").GetComponent<ScoreDisplay>();
        players = FindObjectsOfType<PlayerStateManager>();
        matchActive = true;
        
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
        else if (m_scoringType.playersInGame == 1 && matchActive)
        {
            EndMatch();
        }


        if (m_Animator.GetBool("Results Showing"))  // I hate that this is polling every frame, but it has to in this system [Graham]
        {
            scoreBoard.SetActive(true);
        }
    }

    public void EndMatch()
    {
        // If the match is finished, tell the animator [Graham]
        timer.Hide();
        m_Animator.SetTrigger("End Match");

        //  Cancel all the movements of players 
        foreach (PlayerStateManager player in players)
        {
            player.GetComponent<Rigidbody>().ResetInertiaTensor();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().MovePosition(new Vector3(-6000, -6000, 0));
            player.gameObject.SetActive(false);
        }

        Debug.Log("Match ended");
        backToMenuButton.SetActive(true);
        matchActive = false;


        // If the results are showing, tell the back to menu button to enable itself
        // Shouldn't be in this file, but because it's locked to the animation, 
        // It kinda has to go here. Sorry! [Graham]

        // if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Show Results") && // Somewhat unstable [Graham]
        //     !backToMenuButton.activeInHierarchy)   
        //     backToMenuButton.SetActive(true);
    
    }
}
