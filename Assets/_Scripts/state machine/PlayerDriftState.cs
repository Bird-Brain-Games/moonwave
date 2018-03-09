using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriftState : State
{

    Vector3 m_Direction;
    bool m_CollidedWithPlanet;

    Rigidbody m_RigidBody;
    PlayerStats m_PlayerStats;
    Controls m_Controls;
    Shoot m_Shoot;
    StickToPlanet m_Gravity;

    // Use this for initialization
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_PlayerStats = GetComponent<PlayerStats>();
        m_Controls = GetComponent<Controls>();
        m_Shoot = GetComponent<Shoot>();
        m_Gravity = GetComponent<StickToPlanet>();
    }

    override public void StateEnter()
    {
        m_CollidedWithPlanet = false;
    }

    override public void StateFixedUpdate()
    {
        m_CollidedWithPlanet = false;
    }

    override public void StateUpdate()
    {
        //set the players direction based off of the analog stick
        m_Direction = new Vector3(m_Controls.GetMove().x, m_Controls.GetMove().y, 0.0f);

        // If the stick is being moved, add the force [G, C]
        if (m_Direction.sqrMagnitude > 0.0f)
        {
            //if we are going below max speed add force
            if (m_RigidBody.velocity.magnitude < m_PlayerStats.maxDriftMoveForce)
            {
                m_RigidBody.AddForce(m_PlayerStats.driftMoveForce * m_Direction, ForceMode.Impulse);
            }
            //If we are going above max speed
            else
            {
                float x = 0, y = 0;
                //if we are trying to move in the opposite x direction as our current velocity add that force
                if (m_RigidBody.velocity.x >= 0 && m_Direction.x <= 0)
                    x = m_Direction.x;
                else if (m_RigidBody.velocity.x <= 0 && m_Direction.x >= 0)
                    x = m_Direction.x;

                if (m_RigidBody.velocity.y >= 0 && m_Direction.y <= 0)
                    y = m_Direction.y;
                if (m_RigidBody.velocity.y <= 0 && m_Direction.y >= 0)
                    y = m_Direction.y;


                m_RigidBody.AddForce(m_PlayerStats.driftMoveForce * new Vector3(x, y), ForceMode.Impulse);
            }
        }
        // If the shoot button is pressed, FIRE THE LASER
        if (m_Controls.GetShootLaser())
        {
            m_Shoot.ShootLaser();
        }

        // If the "fire shotgun" button is pressed, shoot it. [Graham]
        if (m_Controls.GetShootShotgun())
        {
            m_Shoot.ShootShotgun();
        }

        // Update the gravity [G, C]
        m_RigidBody.AddForce(m_Gravity.DriftingUpdate() * m_PlayerStats.fallGravMultiplier);
        if (m_CollidedWithPlanet)
        {
            // Change the state to the "Moving on planet" state
            ChangeState(m_PlayerStats.PlayerOnPlanetStateString);
        }

        // Check if boosting
        if (m_Controls.GetBoost(BUTTON_DETECTION.GET_BUTTON) && m_PlayerStats.CanBoost == true)
        {
            // Change the state to the "Boost Charge" state
            ChangeState(m_PlayerStats.PlayerBoostChargeString);
        }
    }

    override public void StateOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            m_CollidedWithPlanet = true;
        }
    }

    override public void StateOnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            m_CollidedWithPlanet = true;
        }
    }
}
