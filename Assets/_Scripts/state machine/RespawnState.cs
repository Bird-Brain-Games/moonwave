using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnState : State {

    //how long it takes to respawn
    public float respawnTime;
    float timer;
    PlayerStats m_PlayerStats;
    Spawn m_spawner;

    // Use this for initialization
    void Start () {
        m_PlayerStats = GetComponent<PlayerStats>();
        m_spawner = GetComponentInParent<Spawn>();
        respawnTime = m_PlayerStats.m_respawnTime;
    }
    override public void StateEnter()
    {
        timer = 0;
    }

    override public void StateExit()
    {
    }

    override
    public void StateUpdate()
    {

        if (timer < respawnTime)
        {   
            timer += Time.deltaTime;
        }
        else
        {
            ChangeState(m_PlayerStats.PlayerDriftStateString);
            transform.position = m_spawner.getSpawnPoint();
        }
    }
}
