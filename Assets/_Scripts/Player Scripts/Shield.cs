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

    public float m_rechargeDelay;
    public int m_rechargeRatePerSecond;

    //[HideInInspector]
    public float m_timeSinceLastHit;
    //[HideInInspector]
    public float m_hitUpdate;

    float m_timeSinceLastRecharge;
    float m_rechargeUpdate;

    //triggers the recharge rate counter to start. set to false when the player is hit
    public bool m_canRecharge;

    // Red Outline
    public Renderer m_playerRenderer;
    public Material m_outlineMaterial;
    private Material[] m_mats;
    private Color m_playerColor;

    // Use this for initialization
    void Start()
    {
        m_canRecharge = false;
        m_shieldHealth = m_maxShieldHealth;
        m_playerStats = GetComponentInParent<PlayerStats>();

        // Set the player's outline color
        SetColor(this.transform.parent.GetComponent<PlayerStats>().colour);
    }

    public void SetColor(Color newColor)
    {
        m_playerColor = newColor;
        m_playerRenderer.materials[1].SetColor("_OutlineColor", m_playerColor);
    }

    //this is called whenever a player is hit and basically resets the shield recharge variables.

    public void ShieldHit(BULLET_TYPE bullet)
    {
        if (m_shieldHealth > 0f)
        {
            float oldHealth = m_shieldHealth;
            switch (bullet)
            {
                case BULLET_TYPE.plasma:
                    m_shieldHealth -= GetComponentInParent<Shoot>().m_bulletImpact;
                    break;
                case BULLET_TYPE.shotgun:
                    m_shieldHealth -= m_playerStats.m_Shoot.shotgunDamage;
                    break;
            }

            if (oldHealth > 0f && m_shieldHealth <= 0f) // The hit that caused the shield to shatter
                StartCoroutine(GetComponentInParent<Controls>().RumbleFor(0.2f, 1.0f));
            else if (m_shieldHealth > 0f)
                StartCoroutine(GetComponentInParent<Controls>().RumbleFor(0.1f, 0.25f));
        }
        if (m_shieldHealth <= 0)
        {

            // SFX
            FindObjectOfType<AudioManager>().Play("Shield Shatter");
            StartCoroutine(GetComponentInParent<Controls>().RumbleFor(0.1f, 0.3f));
            
            // Add the player's outline
            m_playerRenderer.materials[1].SetColor("_OutlineColor", Color.red);
            m_playerRenderer.materials[1].SetFloat("_Outline", 0.15f);

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
                    m_playerRenderer.materials[1].SetFloat("_Outline", Mathf.PingPong(Time.time, 0.15f));
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
                    m_playerRenderer.materials[1].SetFloat("_Outline", Mathf.PingPong(Time.time, 0.3f));
                }
                else
                {
                    //Debug.Log("shield recharge");
                    m_shieldHealth += m_rechargeRatePerSecond;
                    m_playerStats.SetShieldState(true);
                    m_timeSinceLastRecharge = Time.time;

                    // Remove the player's outline
                    m_playerRenderer.materials[1].SetColor("_OutlineColor", m_playerColor);
                    m_playerRenderer.materials[1].SetFloat("_Outline", 0.15f);
                }
            }
        }
    }

    public void ResetShield()
    {
        // Needs more fleshing out, not enough knowledge on this script [Graham]
        m_canRecharge = false;
        m_shieldHealth = m_maxShieldHealth;
        m_timeSinceLastHit = 0f;
        //GetComponent<MeshRenderer>().enabled = true;
        m_playerStats.SetShieldState(true);
        m_playerRenderer.materials[1].SetColor("_OutlineColor", m_playerColor);
    }
}
