using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShot : Projectile {

	float m_Duration;
	float m_CurrentDuration;
	Vector3 m_StartScale;
	Vector3 m_EndScale;
	Vector3 m_ScaleVector;

	// Use this for initialization
	void Start()
	{

		
		m_Duration = m_PlayerStats.m_Shoot.shotgunDuration;
		m_CurrentDuration = m_Duration;

		m_StartScale = transform.localScale;
		m_EndScale = transform.localScale;
		m_EndScale.x *= m_PlayerStats.m_Shoot.shotgunRadius;

		Physics.IgnoreCollision(
                GetComponent<Collider>(), 
                m_PlayerStats.GetComponent<Collider>());
	}
	
	// Update is called once per frame
	void Update () {
		m_ScaleVector = Vector3.Lerp(m_StartScale, m_EndScale, 1 - m_CurrentDuration / m_Duration);
		transform.localScale = m_ScaleVector;

		m_CurrentDuration -= Time.deltaTime;
		if (m_CurrentDuration <= 0f)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
            if (other.transform.GetComponent<PlayerStats>().Invincible == false)
            {
                collideWithPlayer(other);
            }
		}
	}

	void collideWithPlayer(Collider other)
	{
		Shield m_shield = other.gameObject.GetComponentInChildren<Shield>();
		Vector3 addForce;

		other.gameObject.GetComponent<PlayerStats>().m_HitLastBy = m_PlayerStats;

		// If the shield has health, change how much force 
		if (m_shield.m_shieldHealth <= 0)
		{
			addForce = m_Direction * m_Force * m_PlayerStats.m_Shoot.shotgunCriticalMultiplier;
			Debug.Log(addForce);
		}
		else
		{
			addForce = m_Direction * m_Force;
		}

		// Add the force [Graham]
		other.GetComponent<Rigidbody>().AddForce(addForce, ForceMode.Impulse);

		// Tell the shield to be hit
		m_shield.ShieldHit(Shield.BULLET_TYPE.shotgun);
	}
}
