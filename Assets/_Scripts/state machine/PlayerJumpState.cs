using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State {

	Vector3 m_Direction;
	bool m_CollidedWithPlanet;
	float jumpTimer;
	
	Rigidbody m_RigidBody;
	PlayerStats m_PlayerStats;
	Controls m_Controls;
	Shoot m_Shoot;
	StickToPlanet m_Gravity;

	// Use this for initialization
	void Start () {
		m_RigidBody = GetComponent<Rigidbody>();
		m_PlayerStats = GetComponent<PlayerStats>();
		m_Controls = GetComponent<Controls>();
		m_Shoot = GetComponent<Shoot>();
		m_Gravity = GetComponent<StickToPlanet>();
	}

	override public void StateEnter()
	{
		jumpTimer = 0f;
		m_CollidedWithPlanet = false;
	}
	
	// Update is called once per frame
	override public void StateUpdate () {
		//set the players direction based off of the analog stick
        m_Direction = new Vector3(m_Controls.GetMove().x, m_Controls.GetMove().y, 0.0f);
		jumpTimer += Time.deltaTime;

		// If the stick is being moved, add the force [G, C]
		if (m_Direction.sqrMagnitude > 0.0f)
			m_RigidBody.AddForce(m_PlayerStats.driftMoveForce * m_Direction, ForceMode.Impulse);

		// If the jump button is released, change to drift state
		if (m_Controls.GetJump(BUTTON_DETECTION.GET_BUTTON_UP) || jumpTimer > m_PlayerStats.maxJumpTime)
		{
			ChangeState(m_PlayerStats.PlayerDriftStateString);
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
		m_RigidBody.AddForce(m_Gravity.DriftingUpdate());
		if(m_CollidedWithPlanet)
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

	// override public void StateOnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "Planet")
    //     {
    //         m_CollidedWithPlanet = true;
    //     }
    // }
}
