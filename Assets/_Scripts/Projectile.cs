using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

	public Rigidbody m_Rigidbody;
	public PlayerStats m_PlayerStats;

	// Collision Layers [Graham]
	protected int m_PlanetLayer = 8;
	protected int m_PlayerLayer = 9;
    protected int m_SunsLayer = 12;
	protected int m_ProjectileLayer = 10;

	// member variables [Graham]
	protected Vector2 m_Direction;
	protected float m_Force;

	// Used to set the values of the projectile [Graham]
	public void Init(Vector2 a_direction, float a_force, PlayerStats a_PlayerStats)
	{
		m_Direction = a_direction.normalized;
		m_Force = a_force;
		m_PlayerStats = a_PlayerStats;
	}
}
