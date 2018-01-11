using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoostChargeState : State
{

    Vector3 m_Direction;

    PlayerStats m_PlayerStats;
    Controls m_Controls;
    PlayerBoost m_Boost;
    Animator m_Animator;

    // Use this for initialization
    void Start()
    {
        m_PlayerStats = GetComponent<PlayerStats>();
        m_Controls = GetComponent<Controls>();
        m_Boost = GetComponent<PlayerBoost>();
        m_Animator = GetComponentInChildren<Animator>();
    }

    public override void StateEnter()
    {
        m_Boost.EntryBoost();
        m_Animator.SetTrigger("Boost Started");
    }

    override public void StateUpdate()
    {
        //set the players direction based off of the analog stick
        m_Direction = new Vector3(m_Controls.GetMove().x, m_Controls.GetMove().y, 0.0f);

        // Charge the boost [Graham] (pull the lever graham, The other lever)
        m_Boost.ChargeBoost();

        if (m_Controls.GetBoost(BUTTON_DETECTION.GET_BUTTON_UP))
        {
            // If the boost is released, fire the boost [Graham]
            m_Boost.FireBoost();
            ChangeState(m_PlayerStats.PlayerBoostActiveString);
        }
    }
}
