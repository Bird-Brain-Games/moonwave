using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutOfTime : MonoBehaviour {

    public GameObject timerObject;
    public GameObject backToMenuButton;
    Text m_Text;
    Timer timer;
    Animator m_Animator;

	// Use this for initialization
	void Awake () {
        timer = timerObject.GetComponent<Timer>();
        m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        // Update the animator time control (for transitions) [Graham]
        m_Animator.SetFloat("time", timer.GetTime());

        if (m_Animator.GetBool("InGame"))
        {
            // Hacky, but it works for now [Graham]
            timer.Show();
        }

        // If the match is finished, tell the animator [Graham]
        if (timer.OutOfTime())
        {
            timer.Hide();
            m_Animator.SetTrigger("End Match");

            // If the results are showing, tell the back to menu button to enable itself
            // Shouldn't be in this file, but because it's locked to the animation, 
            // It kinda has to go here. Sorry! [Graham]
            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Show Results") && // Somewhat unstable [Graham]
                !backToMenuButton.activeInHierarchy)   
                backToMenuButton.SetActive(true);
        }
    }
}
