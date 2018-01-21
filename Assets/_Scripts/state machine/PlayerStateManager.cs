using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{

    StateManager movementStates;
    PlayerStats playerStats;
    StickToPlanet m_Gravity;
    Rigidbody m_RigidBody;
    Animator m_Animator;

    float stunTimer;
    bool isCountingDown;

    bool m_renableCollisions;
    bool m_roundStart;
    Collider m_c1;
    Collider m_c2;

    #region movementStates
    PlayerDriftState driftState;
    PlayerJumpState jumpState;
    PlayerOnPlanetState onPlanetState;
    PlayerBoostChargeState boostChargeState;
    PlayerBoostActiveState boostActiveState;
    PLayerBigHitState bigHitState;
    RespawnState respawnState;
    #endregion

    void Awake()
    {
        m_roundStart = true;
        m_renableCollisions = true;
        movementStates = gameObject.AddComponent<StateManager>();
        driftState = gameObject.AddComponent<PlayerDriftState>();
        jumpState = gameObject.AddComponent<PlayerJumpState>();
        onPlanetState = gameObject.AddComponent<PlayerOnPlanetState>();
        boostChargeState = gameObject.AddComponent<PlayerBoostChargeState>();
        boostActiveState = gameObject.AddComponent<PlayerBoostActiveState>();
        bigHitState = gameObject.AddComponent<PLayerBigHitState>();
        respawnState = gameObject.AddComponent<RespawnState>();
    }

    // Use this for initialization
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        m_Gravity = GetComponent<StickToPlanet>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponentInChildren<Animator>();

        movementStates.AttachDefaultState(playerStats.PlayerDriftStateString, driftState);
        movementStates.AttachState(playerStats.PlayerJumpStateString, jumpState);
        movementStates.AttachState(playerStats.PlayerOnPlanetStateString, onPlanetState);
        movementStates.AttachState(playerStats.PlayerBoostActiveString, boostActiveState);
        movementStates.AttachState(playerStats.PlayerBoostChargeString, boostChargeState);
        movementStates.AttachState(playerStats.PlayerBigHitState, bigHitState);
        movementStates.AttachState(playerStats.PlayerRespawnState, respawnState);

        stunTimer = 3f;
        movementStates.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the trigger is sent to stun the player
        if (playerStats.stunTrigger)
        {
            StunPlayer();
            m_Animator.SetTrigger("Hurt");
        }

        // If the player is stunned
        if (stunTimer > 0f)
        {
            // Update the stun timer [Graham]
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                UnStunPlayer();
            }

            // Update the animator
            m_Animator.SetFloat("Stun Timer", stunTimer);

            // Cause the stunned player to be affected by gravity[Graham]
            if (m_roundStart)
            {
                m_RigidBody.AddForce(m_Gravity.DriftingUpdate() * 0.5f);

            }
        }
    }

    public void ResetPlayer()
    {
        UnStunPlayer();
        movementStates.ChangeState(playerStats.PlayerRespawnState);
    }

    void StunPlayer()
    {
        playerStats.stunTrigger = false;
        stunTimer = playerStats.StunTimer;
        movementStates.enabled = false;
        GetComponent<Collider>().material.bounciness = 1.0f;
        GetComponent<Collider>().material.bounceCombine = PhysicMaterialCombine.Maximum;
    }

    void UnStunPlayer()
    {
        movementStates.enabled = true;
        movementStates.ResetToDefaultState();
        stunTimer = 0f;
        m_Animator.SetFloat("Stun Timer", stunTimer);
        GetComponent<Collider>().material.bounciness = 0.0f;
        GetComponent<Collider>().material.bounceCombine = PhysicMaterialCombine.Average;
        m_roundStart = false;
        if (m_renableCollisions == false)
        {
            Physics.IgnoreCollision(m_c1, m_c2, m_renableCollisions);
            m_renableCollisions = true;
        }
    }

    public void SetCollider(Collider c1, Collider c2)
    {
        m_c1 = c1;
        m_c2 = c2;
        m_renableCollisions = false;
    }

    public void ChangeState(string a_State)
    {
        movementStates.ChangeState(a_State);
    }

    public void resetRound()
    {
        m_roundStart = true;
    }
}
