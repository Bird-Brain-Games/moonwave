using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnPlanetState : State {

	Vector3 m_Direction;

	Animator m_Animator;
	Rigidbody m_RigidBody;
	PlayerStats m_PlayerStats;
	Controls m_Controls;
	Shoot m_Shoot;
	StickToPlanet m_Gravity;
	PlayerMoveOnPlanet m_Move;

	// Use this for initialization
	void Start () {
		m_RigidBody = GetComponent<Rigidbody>();
		m_PlayerStats = GetComponent<PlayerStats>();
		m_Controls = GetComponent<Controls>();
		m_Shoot = GetComponent<Shoot>();
		m_Gravity = GetComponent<StickToPlanet>();
		m_Move = GetComponent<PlayerMoveOnPlanet>();
		m_Animator = GetComponentInChildren<Animator>();
	}

	override public void StateEnter()
	{
		m_Animator.SetBool("OnPlanet", true);
	}

	override public void StateUpdate()
	{
		// Get the player input
        //set the players direction based off of the analog stick
        m_Direction = new Vector3(m_Controls.GetMove().x, m_Controls.GetMove().y, 0.0f);

		// Update the gravity [G, C]
		m_Gravity.GroundedUpdate();

		// If the stick is being moved, add the force [G, C]
		if (m_Direction.sqrMagnitude > 0.0f)
			m_Move.MoveOnPlanet();
		
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

		// If the jump button is pressed, cause the player to jump
		if (m_Controls.GetJump(BUTTON_DETECTION.GET_BUTTON_DOWN))
		{
			m_RigidBody.AddForce(m_PlayerStats.jumpForce * transform.up, ForceMode.Impulse);
			ChangeState(m_PlayerStats.PlayerJumpStateString);
		}

		// If it can't find ground beneath it, change to drifting state
		if (!m_Gravity.FindIfGrounded())
		{
			ChangeState(m_PlayerStats.PlayerDriftStateString);
			Debug.Log("No ground found in On Planet State");
		}

		// Check if boosting
		if (m_Controls.GetBoost(BUTTON_DETECTION.GET_BUTTON) && m_PlayerStats.CanBoost == true)
		{
			// Change the state to the "Boost Charge" state
			ChangeState(m_PlayerStats.PlayerBoostChargeString);
		}

		// Update the animator properties
		m_Animator.SetFloat("WalkSpeed", m_RigidBody.velocity.magnitude);
	}

	override public void StateExit()
	{
		m_Animator.SetBool("OnPlanet", false);
		m_Animator.SetFloat("WalkSpeed", 0f);
	}
}
