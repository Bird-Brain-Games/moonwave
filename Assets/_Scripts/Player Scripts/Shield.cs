using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Shield : MonoBehaviour
{

    public enum BULLET_TYPE
    {
        plasma = 0,
        shotgun = 1
    }

    PlayerStats m_playerStats;

    //self explanitory
    public float m_shieldHealth;
    public float m_maxShieldHealth;
    public float m_percentSheild;

    public float m_rechargeDelay;
    public int m_rechargeRatePerSecond;

    public Color m_maxColor;
    public Color m_currentColor;
    public Color m_endColor;

    public GameObject m_Explosion;

    //[HideInInspector]
    public float m_timeSinceLastHit;
    //[HideInInspector]
    public float m_hitUpdate;

    float m_timeSinceLastRecharge;
    float m_rechargeUpdate;

    //triggers the recharge rate counter to start. set to false when the player is hit
    public bool m_canRecharge;

    // Use this for initialization
    void Start()
    {
        m_canRecharge = false;
        m_shieldHealth = m_maxShieldHealth;
        m_playerStats = GetComponentInParent<PlayerStats>();
        m_currentColor = m_maxColor;
    }

    //this is called whenever a player is hit and basically resets the shield recharge variables.

    public void ShieldHit(BULLET_TYPE bullet)
    {
        if (m_shieldHealth > 0)
        {
            switch (bullet)
            {
                case BULLET_TYPE.plasma:
                    m_shieldHealth -= GetComponentInParent<Shoot>().m_bulletImpact;
                    break;
                case BULLET_TYPE.shotgun:
                    m_shieldHealth -= m_playerStats.m_Shoot.shotgunDamage;
                    break;
            }

        }
        if (m_shieldHealth <= 0)
        {
            if (GetComponent<MeshRenderer>().enabled)
            {
                // SFX
                FindObjectOfType<AudioManager>().Play("Shield Shatter");
                Instantiate(m_Explosion, transform);
            }
           
            m_shieldHealth = 0;
            GetComponent<MeshRenderer>().enabled = false;
            m_playerStats.SetShieldState(false);

            // Stun the player if they are hit when shield down
            if (bullet == BULLET_TYPE.shotgun)
            {
                m_playerStats.stunTrigger = true;
                m_playerStats.StunTimer = m_playerStats.maxShotgunStunTime;
            }
        }
        m_timeSinceLastHit = Time.time;
        m_hitUpdate = Time.time;
        m_canRecharge = false;
    }

    // Update is called once per frame
    void Update()
    {

        //a blanket if statement to reduce the times that this code will run. aka only if they are injured.
        if (m_maxShieldHealth > m_shieldHealth)
        {
            //Debug.Log("shield updating");
            //controls whether enough time has passed for the shield to start recharging
            if (m_canRecharge == false)
            {
                //controls the timer updates and whether enough time has passed or not.
                if (m_hitUpdate - m_timeSinceLastHit < m_rechargeDelay)
                {
                    //Debug.Log("hit update");
                    m_hitUpdate = Time.time;
                }
                else
                {
                    //Debug.Log("shield can recharge");
                    m_canRecharge = true;
                    m_timeSinceLastRecharge = Time.time;
                }
            }
            //If the recharge delay has passed then the shield will start recharging.
            else
            {
                if (0.5 > m_rechargeUpdate - m_timeSinceLastRecharge)
                {
                    m_rechargeUpdate = Time.time;
                }
                else
                {
                    //Debug.Log("shield recharge");
                    m_shieldHealth += m_rechargeRatePerSecond;
                    GetComponent<MeshRenderer>().enabled = true;
                    m_playerStats.SetShieldState(true);
                    m_timeSinceLastRecharge = Time.time;
                }
            }
        }

        // Calculate % Damage [Jack]
        m_percentSheild = m_shieldHealth / m_maxShieldHealth;

        // Adjust the sheild color based on % damnge taken! [Jack]

        m_currentColor.b = Mathf.Lerp(m_endColor.b, m_maxColor.b, m_percentSheild);
        m_currentColor.r = Mathf.Lerp(m_endColor.r, m_maxColor.r, m_percentSheild);
        m_currentColor.g = Mathf.Lerp(m_endColor.g, m_maxColor.g, m_percentSheild);
        m_currentColor.a = Mathf.Lerp(m_endColor.a, m_maxColor.a, m_percentSheild);

        gameObject.GetComponent<Renderer>().material.color = m_currentColor;
    }

    public void ResetShield()
    {
        // Needs more fleshing out, not enough knowledge on this script [Graham]
        m_canRecharge = false;
        m_shieldHealth = m_maxShieldHealth;
        m_timeSinceLastHit = 0f;
        GetComponent<MeshRenderer>().enabled = true;
        m_playerStats.SetShieldState(true);
        m_currentColor = m_maxColor;
    }
}
