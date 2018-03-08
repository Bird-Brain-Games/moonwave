using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    #region variables 
    //players rigidBody
    public Rigidbody bullet;

    // Animator component of the player
    Animator m_Animator;

    //The impact a bullet has on a player
    public float m_bulletImpact;

    //Whether we can shoot on this frame, is affected by m_bulletDelay
    bool m_shootTimer;
    
    //The accessers to our controller script
    Controls controls;
    Shotgun m_Shotgun;
    Vector2 aimDir, moveDir;
    public float m_bulletSpeed;

    //Variable used to control how fast bullets can be shot
    public float m_bulletDelay;
    //Timer variables
    float m_timer;
    float m_startTime;

    //Variables for Bullet spread [Jack, Robbie]
    public int m_stray;  // Control the variable for the spread, a higher number is greater variance
    float m_randomX;
    float m_randomY;

    // Logging [Jack]
    public int l_bullets; // using l_ to indicate this data is for logging

    // Power Up shot variables
    public int m_shotType = 1;
    const int NORMAL = 1;
    const int TRIPLE = 2;
    const int GIANT = 3;

    PlayerStats m_playerStats;

    #endregion
    
    void Start () {
        controls = GetComponent<Controls>();
        m_playerStats = GetComponent<PlayerStats>();
        m_Shotgun = GetComponent<Shotgun>();
        m_Animator = GetComponentInChildren<Animator>();
        l_bullets = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (m_bulletDelay > m_timer - m_startTime && !m_shootTimer)
        {
            m_timer = Time.time;
            //Debug.Log("start Time: " + m_startTime);
            //Debug.Log("Time: " + m_timer);
        }
        else if (m_bulletDelay < m_timer - m_startTime && !m_shootTimer)
        {
            //Debug.Log("shoot recharged");
            m_shootTimer = true;
            m_timer = 0;
            m_startTime = 0;
        }

        aimDir = controls.GetAim();
        moveDir = controls.GetMove();

        m_Animator.SetBool("Shooting Laser", false);
    }

    public void ShootLaser()
    {
       // If the timer allows us to shoot again
        if (m_shootTimer)
        {
            // SFX
            if (m_playerStats.m_PlayerID == 1) { FindObjectOfType<AudioManager>().Play("Pew"); }
            else if (m_playerStats.m_PlayerID == 2) { FindObjectOfType<AudioManager>().Play("Pew2"); }
            else if (m_playerStats.m_PlayerID == 2) { FindObjectOfType<AudioManager>().Play("Pew3"); }
            else { FindObjectOfType<AudioManager>().Play("Pew4"); }

            m_shootTimer = false;
            m_timer = Time.time;
            m_startTime = Time.time;

            // Based on Shot Type
            switch(m_shotType)
            {
                case NORMAL:
                    NormalShot();
                break;

                case TRIPLE:
                    TripleShot();
                break;

                case GIANT:
                    GiantShot();
                break;
            }

            
        // Log total shots fired [Jack]
        l_bullets++; // Take a note of how many player shots
        
        // Tell the animator to fire the bullet
        m_Animator.SetBool("Shooting Laser", true);
        }
    }

    public void ShootShotgun()
    {
        
        m_Shotgun.Shoot();

        float shootingAngle = (aimDir.y + 1) / 2;

    }

    ///////////// Plasma Shot Types //////////////

        public void NormalShot()
    {

           // Bullet spread calculation [Jack]
            m_randomX = Random.Range(-m_stray, m_stray);
            m_randomY = Random.Range(-m_stray, m_stray);
            m_randomY = m_randomY / 100;
            m_randomX = m_randomX / 100;

            // Bullet Spread applied by adding the random values to the aim
            if (aimDir.sqrMagnitude == 0f) 
            {
                if (moveDir.sqrMagnitude == 0f)
                    aimDir = transform.up;	// If not aiming or moving, fire straight up
                else
                {
                    aimDir = moveDir;
                }
                    
            }
            Vector3 forward = new Vector3(aimDir.x + m_randomX, aimDir.y + m_randomY);
            forward.Normalize();
            //Quaternion rotation = Quaternion.LookRotation(transform.f, aimDir);

            //creating the bullet
            Quaternion rotation = Quaternion.LookRotation(transform.forward, forward);
            Rigidbody clone = Instantiate(bullet, transform.position + (forward*2.5f), rotation);          
            clone.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));

            //setting the bullets speed
            //forward *= m_bulletSpeed;
            clone.velocity = forward * m_bulletSpeed;
                        
            // Initialize the bullet
            clone.GetComponent<Bullet>().Init(
                forward, m_bulletImpact, m_playerStats);
            
            Physics.IgnoreCollision(
                clone.GetComponent<Collider>(), 
                GetComponent<Collider>());
            

            clone.GetComponent<Bullet>().m_bulletParticles.m_spriteColour = (COLOUR)m_playerStats.m_PlayerID;
            
            
            //clone.GetComponent<Bullet>().setVelocity(clone.velocity);
            clone.GetComponent<MeshRenderer>().material.color = m_playerStats.ColourOfBullet;
            

            // Log total shots fired [Jack]
            l_bullets++; // Take a note of how many player shots
        
    }
    
    public void TripleShot()
    {        
        // Bullet spread calculation [Jack]
        m_randomX = Random.Range(-m_stray, m_stray);
        m_randomY = Random.Range(-m_stray, m_stray);
        m_randomY = m_randomY / 100;
        m_randomX = m_randomX / 100;

        // Bullet Spread applied by adding the random values to the aim
        if (aimDir.sqrMagnitude == 0f) 
        {
            if (moveDir.sqrMagnitude == 0f)
                aimDir = transform.up;	// If not aiming or moving, fire straight up
            else
            {
                aimDir = moveDir;
            }
                    
        }
        Vector3 forward = new Vector3(aimDir.x + m_randomX, aimDir.y + m_randomY);
        forward.Normalize();
        //Quaternion rotation = Quaternion.LookRotation(transform.f, aimDir);

        //creating the bullet
        Quaternion rotation = Quaternion.LookRotation(transform.forward, forward);
        Rigidbody clone = Instantiate(bullet, transform.position + (forward*2.5f), rotation);
        Rigidbody clone2 = Instantiate(bullet, transform.position + 
            new Vector3(3f, 0f, 0f) + (forward * 2.0f), rotation);
        Rigidbody clone3 = Instantiate(bullet, transform.position + 
            new Vector3(-3f, 0f, 0f) + (forward * 2.0f), rotation);
            
        clone.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        clone2.transform.Rotate(new Vector3(0.0f, 0.0f, 83.0f));
        clone3.transform.Rotate(new Vector3(0.0f, 0.0f, 97.0f));

        //setting the bullets speed
        //forward *= m_bulletSpeed;
        clone.velocity = forward * m_bulletSpeed;
        clone2.velocity = (forward + new Vector3( 0.1f, 0f, 0f)) * m_bulletSpeed;
        clone3.velocity = (forward + new Vector3( -0.1f, 0f, 0f)) * m_bulletSpeed;

        // Initialize the bullet
        clone.GetComponent<Bullet>().Init(
            forward, m_bulletImpact, m_playerStats);
            
        Physics.IgnoreCollision(
            clone.GetComponent<Collider>(), 
            GetComponent<Collider>());
        Physics.IgnoreCollision(
            clone2.GetComponent<Collider>(), 
            GetComponent<Collider>());
        Physics.IgnoreCollision(
            clone3.GetComponent<Collider>(), 
            GetComponent<Collider>());

        clone.GetComponent<Bullet>().m_bulletParticles.m_spriteColour = (COLOUR)m_playerStats.m_PlayerID;
        clone2.GetComponent<Bullet>().m_bulletParticles.m_spriteColour = (COLOUR)m_playerStats.m_PlayerID;
        clone3.GetComponent<Bullet>().m_bulletParticles.m_spriteColour = (COLOUR)m_playerStats.m_PlayerID;
            
        //clone.GetComponent<Bullet>().setVelocity(clone.velocity);
        clone.GetComponent<MeshRenderer>().material.color = m_playerStats.ColourOfBullet;
        clone2.GetComponent<MeshRenderer>().material.color = m_playerStats.ColourOfBullet;
        clone3.GetComponent<MeshRenderer>().material.color = m_playerStats.ColourOfBullet;

    }

            public void GiantShot()
    {

           // Bullet spread calculation [Jack]
            m_randomX = Random.Range(-m_stray, m_stray);
            m_randomY = Random.Range(-m_stray, m_stray);
            m_randomY = m_randomY / 100;
            m_randomX = m_randomX / 100;

            // Bullet Spread applied by adding the random values to the aim
            if (aimDir.sqrMagnitude == 0f) 
            {
                if (moveDir.sqrMagnitude == 0f)
                    aimDir = transform.up;	// If not aiming or moving, fire straight up
                else
                {
                    aimDir = moveDir;
                }
                    
            }
            Vector3 forward = new Vector3(aimDir.x + m_randomX, aimDir.y + m_randomY);
            forward.Normalize();
            //Quaternion rotation = Quaternion.LookRotation(transform.f, aimDir);

            //creating the bullet
            Quaternion rotation = Quaternion.LookRotation(transform.forward, forward);
            Rigidbody clone = Instantiate(bullet, transform.position + (forward*2.5f), rotation);          
            clone.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            clone.transform.localScale = new Vector3(16.0f, 8.0f, 5.0f);
            
            //setting the bullets speed
            //forward *= m_bulletSpeed;
            clone.velocity = forward * m_bulletSpeed;
                        
            // Initialize the bullet
            clone.GetComponent<Bullet>().Init(
                forward, m_bulletImpact, m_playerStats);
            
            Physics.IgnoreCollision(
                clone.GetComponent<Collider>(), 
                GetComponent<Collider>());
            

            clone.GetComponent<Bullet>().m_bulletParticles.m_spriteColour = (COLOUR)m_playerStats.m_PlayerID;
            
            
            //clone.GetComponent<Bullet>().setVelocity(clone.velocity);
            clone.GetComponent<MeshRenderer>().material.color = m_playerStats.ColourOfBullet;
            

            // Log total shots fired [Jack]
            l_bullets++; // Take a note of how many player shots
        
    }

}