using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    Text m_Text;
    bool m_OutOfTime;
    bool m_Paused;
    public float m_MaxTime;
    float m_Time;
    

	// Use this for initialization
	void Start () {
        m_Text = GetComponent<Text>();
        StartTimer();
        Hide();
	}
	
	// Update is called once per frame
	void Update () {
        // Reduce the timer
        if (!m_OutOfTime && !m_Paused)
        {
            m_Time -= Time.deltaTime;
            if (m_Time < 1.0f)
            {
                m_Time = 0.0f;
                m_OutOfTime = true;
                //TODO reset playerStateManagers

            }
        }

        // Update the timer text
        if (((int)m_Time) % 60 < 10)
            m_Text.text = (((int)m_Time) / 60) + ":0" + (((int)m_Time) % 60);
        else
            m_Text.text = (((int)m_Time) / 60) + ":" + (((int)m_Time) % 60);
    }

    public bool OutOfTime()
    {
        return m_OutOfTime;
    }

    /// <summary>
    /// Start the timer 
    /// </summary>
    public void StartTimer() //[Graham]
    {
        m_Time = m_MaxTime;
        m_Paused = false;
        m_OutOfTime = false;
    }

    /// <summary>
    /// Pause the timer [Graham]
    /// </summary>
    public void Pause()
    {
        m_Paused = true;
    }

    public bool IsPaused()
    {
        return m_Paused;
    }

    public void Resume()
    {
        m_Paused = false;
    }

    // Hide and pause the timer [Graham]
    public void Hide()
    {
        Pause();
        m_Text.enabled = false;
    }

    public void Show()
    {
        Resume();
        m_Text.enabled = true;
    }

    public float GetTime()
    {
        return m_Time;
    }
}
