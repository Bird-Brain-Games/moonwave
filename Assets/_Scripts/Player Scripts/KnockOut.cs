using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOut : MonoBehaviour {

 
    Rigidbody m_rigidBody;
    PlayerStats m_PlayerStats;
    PlayerStateManager m_StateManager;
    Shield m_shield;

    // Logging    l_ is used to indicate a variable is a logging variable
    public int l_deaths;
	// Use this for initialization
	void Start () {
        m_rigidBody = GetComponent<Rigidbody>();
        m_PlayerStats = GetComponent<PlayerStats>();
        m_StateManager = GetComponent<PlayerStateManager>();
        m_shield = GetComponentInChildren<Shield>();
        l_deaths = 0;
    }
	

	public void PlayerKnockedOut ()
    {
        Debug.Log("death");
        // SFX
        if (m_PlayerStats.m_PlayerID == 1) { FindObjectOfType<AudioManager>().Play("Death"); }
        else if (m_PlayerStats.m_PlayerID == 2) { FindObjectOfType<AudioManager>().Play("Death2"); }
        else if (m_PlayerStats.m_PlayerID == 3) { FindObjectOfType<AudioManager>().Play("Death3"); }
        else { FindObjectOfType<AudioManager>().Play("Death4"); }

        // The player who hit them out gets 2 points [Jack]
        if (m_PlayerStats.m_HitLastBy != null)
        {
            m_PlayerStats.m_HitLastBy.m_Score += 2;

            // Log who player was killed by [Jack]
            m_PlayerStats.l_killedBy[m_PlayerStats.m_HitLastBy.m_PlayerID]++;
        }
        else
        {
            // Log that the player killed themself [Jack]
            m_PlayerStats.l_killedBy[m_PlayerStats.m_PlayerID]++;
        }

        // The player who died loses a point [Jack]
        m_PlayerStats.m_Score--;

        // Reset m_HitLastBy for respawning [Jack]
        m_PlayerStats.m_HitLastBy = null;

        
        ResetPlayer();
        

        // Logging
        l_deaths++;
    }

    public void ResetPlayer()
    {
        m_rigidBody.ResetInertiaTensor();
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.position = new Vector3(200, 200, 200);
        m_shield.ResetShield();
        m_StateManager.ResetPlayer();
    }
}
