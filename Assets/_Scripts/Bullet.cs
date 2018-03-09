using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public GameObject m_particleManager;
    public Rigidbody m_rigidbody;
    public StickToPlanet m_StickToPlanet;
    public BulletParticles m_bulletParticles { get; set; }
    public float m_fallGravMultiplier;
    public StickToPlanet m_Gravity;

    public bool m_gravityEnabled;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_particleManager = Instantiate<GameObject>(m_particleManager);
        m_bulletParticles = m_particleManager.GetComponent<BulletParticles>();
        m_bulletParticles.transform.position = transform.position;
        m_bulletParticles.velocity = -GetComponent<Rigidbody>().velocity / 5;
        m_bulletParticles.random = -m_bulletParticles.velocity.normalized;
        m_rigidbody = GetComponent<Rigidbody>();
        m_StickToPlanet = GetComponent<StickToPlanet>();
        m_Gravity = GetComponent<StickToPlanet>();
    }




    private void Update()
    {
        m_bulletParticles.transform.position = transform.position;
        if (true == m_gravityEnabled)
        {
            //rotate towards planet
            if (m_StickToPlanet.PlanetInRange())
                m_StickToPlanet.RotateTowardsCurrentPlanet();

            //update forces on bullet based on rotation
            m_Rigidbody.AddForce(m_Gravity.DriftingUpdate() * m_fallGravMultiplier / 100000000);

            //unrotate bullet
            Quaternion rotateToVelocity = Quaternion.LookRotation(transform.forward, m_Direction);
            transform.rotation = Quaternion.RotateTowards(m_rigidbody.rotation, rotateToVelocity, 180.0f);
            transform.rotation *= Quaternion.Euler(0, 0, 90);
        }
        m_Direction = m_Rigidbody.velocity.normalized;
    }

    public void BulletOutOfBounds()
    {
        //Debug.Log("bullet out of bounds");
        Destroy(gameObject, 0.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        //sets what layer we have collided with
        int layer = collision.gameObject.layer;

        // If the bullet collides with a planet, destroy it [Robbie]
        if (layer == m_PlanetLayer || layer == m_SunsLayer) // If the object is a planet
        {
            Destroy(gameObject, 0);  // Destroys bullets when they hit a planet
        }

        //if we have collided with a player.
        else if (layer == m_PlayerLayer)
        {
            collideWithPlayer(collision);
            Destroy(gameObject, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    void collideWithPlayer(Collision other)
    {
        if (other.transform.GetComponent<PlayerStats>().Invincible == false)
        {
            Shield m_shield = other.gameObject.GetComponentInChildren<Shield>();
            Vector3 addForce;



            // If the shield has health, change how much force 
            if (m_shield.m_shieldHealth == 0)
            {
                addForce = m_Direction * m_Force * m_PlayerStats.m_CriticalMultipier;
            }
            else
            {
                addForce = m_Direction * m_Force;
            }

            // Add the force [Graham]
            other.gameObject.GetComponent<Rigidbody>().AddForce(addForce, ForceMode.Impulse);

            // Tell the shield to be hit
            m_shield.ShieldHit(Shield.BULLET_TYPE.plasma);
        }
    }

    private void OnDestroy()
    {
        m_bulletParticles.Alive = false;
    }

    public void SetDirection(Vector3 dir)
    {
        m_Direction = dir;
    }
}
