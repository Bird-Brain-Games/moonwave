using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour {

	public ShotgunShot shotgunShot;
    Controls controls;
	PlayerStats m_PlayerStats;
	int m_PlayerNum;
	Vector3 aimDir;
    // Animator component of the player
    Animator m_Animator;

    float m_ShotgunCurrentCooldown;	// Cooldown until you can fire the shotgun again [Graham]
	float m_ShotgunMaxCooldown;

	float m_ShotDistance;
	float m_ShotDuration;
	float m_ShotForce;
	
	// Use this for initialization
	void Start () {
		controls = GetComponent<Controls>();
		m_PlayerStats = GetComponent<PlayerStats>();
        m_Animator = GetComponentInChildren<Animator>();

        m_ShotgunMaxCooldown = m_PlayerStats.m_Shoot.shotgunCooldown;
		m_ShotDistance = m_PlayerStats.m_Shoot.shotgunDistance;
		m_ShotDuration = m_PlayerStats.m_Shoot.shotgunDuration;
		m_ShotForce = m_PlayerStats.m_Shoot.shotgunForce;
		m_PlayerNum = m_PlayerStats.m_PlayerID;
	}
	
	// Update is called once per frame
	void Update () {
		// Update the cooldown timer [Graham]
		if (m_ShotgunCurrentCooldown > 0f)
		{
			m_ShotgunCurrentCooldown -= Time.deltaTime;
			if (m_ShotgunCurrentCooldown < 0f)
				m_ShotgunCurrentCooldown = 0f;
		}
	}

	public void Shoot()
	{
		if (m_ShotgunCurrentCooldown != 0f) return;	// Don't allow shoot if in cooldown [Graham]

        Debug.Log("Shotgun");
        // SFX
        FindObjectOfType<AudioManager>().Play("Shotgun");


        // Tell the animator to fire the bullet
        m_Animator.SetTrigger("Shoot Shotgun");


        // Get the rotation of the object [Graham]
        aimDir = controls.GetAim();
		if (aimDir.sqrMagnitude == 0f) 
			{
				if (controls.GetMove().sqrMagnitude == 0f)
					aimDir = transform.up;	// If not aiming or moving, fire straight up
				else
					aimDir = controls.GetMove();
			}

		Quaternion rotation = Quaternion.LookRotation(transform.forward, aimDir);

		// Instatntiate the shotgun wave [Graham]
		ShotgunShot clone = Instantiate(shotgunShot, transform.position, rotation);
		
		// Figure out the velocity that the shotgun wave should travel at [Graham]
		Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;

		// Figure out how fast the shot should be moving to reach the distance in the given time [Graham]
		float unitsPerSec = m_PlayerStats.m_Shoot.shotgunDistance / m_PlayerStats.m_Shoot.shotgunDuration;

		clone.GetComponent<Rigidbody>().AddForce(currentVelocity + aimDir * unitsPerSec, ForceMode.Impulse);

		// Initialize the shotgun wave [Graham]
		clone.Init(aimDir, m_PlayerStats.m_Shoot.shotgunForce, m_PlayerStats);
		// Physics.IgnoreCollision(
        //         clone.GetComponent<Collider>(), 
        //         GetComponent<Collider>());

		// Reset the cooldown
		m_ShotgunCurrentCooldown = m_ShotgunMaxCooldown;
	}
}
