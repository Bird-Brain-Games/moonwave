using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [System.Serializable]
    public struct Boost
    {
        //The default boost force
        public float BaseForce;
        //The force that is added for every second of charge
        public float AddedForcePerSecond;
        //The max force a boost can have
        public float MaxForce;
        //The default boost duration
        public float BaseDuration;
        //the added time a boost lasts for, for every second of charge
        public float AddedDurationPerSecond;
        //The max duration a boost can last for
        public float MaxDuration;
        //The time it takes for a boost to be ready agian.
        public float Cooldown;
        public float timeToMaxCharge;
        public float boostCriticalHit;
    }

    [System.Serializable]
    public struct Shoot
    {
        // Shotgun based variables
        [Tooltip("The time (s) it takes to reach maximum size and disappear")]
        public float shotgunDuration;
        [Tooltip("The length of time (s) between shotgun shots")]
        public float shotgunCooldown;
        [Tooltip("The force of the shotgun bullets on the player")]
        public float shotgunForce;
        [Tooltip("How far the shotgun shot travels")]
        public float shotgunDistance;
        [Tooltip("The size of the cone of effect")]
        public float shotgunRadius;
        public float shotgunCriticalMultiplier;
        public float shotgunDamage;
    }

    #region  StateStrings
    public string PlayerOnPlanetStateString { get; set; }
    public string PlayerDriftStateString { get; set; }
    public string PlayerJumpStateString {get; set;}
    public string PlayerBoostChargeString { get; set; }
    public string PlayerBoostActiveString { get; set; }
    public string PlayerBigHitState {get; set;}
    public string PlayerRespawnState { get; set; }
    #endregion

    public float m_respawnTime;
    public Color colourdull;

    // Color selction stuff
    public Color colour { get; set; }
    public ColourData colourData;
    public int colourItr;
    public bool canChangeColour;

    // Lobby states [Jack]
    public bool playerSelecting;
    public bool playerConfirmed;

    // Spawn location [Jack]
    public Transform spawnTransform;
    public Vector3 defaultSpawn;

    //A colour for our bullets [cam]
    public Color ColourOfBullet { get; set; }
    public bool Invincible { get; set; }

    // Score Calculations
    public int m_PlayerID;
    public PlayerStats m_HitLastBy;
    public int m_Score;
    public int m_lives;

    // Killed by (Logging) [Jack]
    public int[] l_killedBy;

    // Health variables
    public bool m_shieldState;
    public float m_CriticalMultipier;

    // Drift based variables
    public float driftMoveForce;
    public float maxDriftMoveForce;
    public float walkMoveForce;

    // Jump based variables
    public float jumpForce;
    public float maxJumpTime;
    public float fallGravMultiplier;

    //these are used to set stunTimer;
	public float maxShotgunStunTime;
    public float maxBoostStunTime;

    //This is what is fetched by stun timer.
    public float StunTimer { get; set; }

    public Shoot m_Shoot;
    public Boost m_boost;

    //Determines whether we can boost or not.
    public bool CanBoost { get; set; }

    // Stunned variables [Graham]
    public bool stunTrigger {get; set;}



	void Awake () {
        m_HitLastBy = null;
        m_Score = 0;
        m_shieldState = true;
        CanBoost = true;
        l_killedBy = new int[4];
        stunTrigger = false;
        Invincible = false;

        // lobby system bools [Jack]
        canChangeColour = true;
        playerSelecting = false;
        playerConfirmed = false;

        // Set spawn location [Jack]
        //spawnTransform.position = defaultSpawn;

        // Making them small strings, easier to compare (probably change to ints) [Graham]
        PlayerOnPlanetStateString = "onPlanet";
        PlayerDriftStateString = "drift";
        PlayerBoostActiveString = "boostActive";
        PlayerBoostChargeString = "boostCharge";
        PlayerJumpStateString = "jump";
        PlayerBigHitState = "bigHit";
        PlayerRespawnState = "respawnState";

        //Request a colour from our bullet colours [cam]
        Debug.Log("Player Made");
    }
    private void Start()
    {
        ColourData temp = GetComponentInParent<bulletColour>().SpawnColour();
        if (temp.isFree == false && MatchSettings.numPlayers == 0)  // on character select [Graham]
        {
            Debug.Log("Setting Colour");
            ColourOfBullet = temp.colour;
            colour = temp.colour;
            colourData = temp;


            GetComponentInChildren<SkinnedMeshRenderer>().material.color = colour;
            m_PlayerID = temp.playerID;
            GetComponentInParent<Unique>().GetComponentInChildren<BoostCollider>().setColour(colour);
            colourdull = colour;
            colourItr = colourData.itr;
        }
        else
        {
            Debug.Log("Setting Colour from match settings");
            ColourOfBullet = colour;
            colourdull = colour;
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = colour;
            GetComponentInParent<Unique>().GetComponentInChildren<BoostCollider>().setColour(colour);
        }
        
    }
    //Getters and Setters

    public void ResetPlayer()
    {

    }

    public int getScore()
    {
        return m_Score;
    }
    public int getLives()
    {
        return m_lives;
    }
    
    public bool GetShieldState()
    {
        return m_shieldState;
    }

    public void SetShieldState(bool p_state)
    {
        m_shieldState = p_state;
    }

    public float GetCriticalMultiplier()
    {
        //if shield is active return 1 as multiplier
        
        if (m_shieldState == true)
        {
            //Debug.Log("Critical fail");
            return 1;
        }
        //if shield is deactivated return critical multiplier.
        else
        {
            return m_CriticalMultipier;
        }
    }

    // Select the colour in lobby
    public Color selectColourRight()
    {
        //if (colourData.itr == 6)
        //{
        //    colourData.itr = 0;
        //}

        ColourData temp = GetComponentInParent<bulletColour>().GetNextAvailableColour(colourItr);
        if (temp.isFree == true)
        {
            colourData = temp;
            //Debug.Log("Setting Colour");
            ColourOfBullet = temp.colour;
            colour = temp.colour;
            colourdull = temp.colour;
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = colour;
            GetComponentInParent<Unique>().GetComponentInChildren<BoostCollider>().setColour(colour);
            colourItr = colourData.itr;
        }

        return colour;
    }
    public Color selectColourLeft()
    {
        //if (colourData.itr == 6)
        //{
        //    colourData.itr = 0;
        //}

        ColourData temp = GetComponentInParent<bulletColour>().GetPreviousAvailableColour(colourItr);
        if (temp.isFree == true)
        {
            colourData = temp;
            //Debug.Log("Setting Colour");
            ColourOfBullet = temp.colour;
            colour = temp.colour;
            colourdull = temp.colour;
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = colour;
            GetComponentInParent<Unique>().GetComponentInChildren<BoostCollider>().setColour(colour);
            colourItr = colourData.itr;
        }

        return colour;
    }

    public void confirmColor()
    {
        GetComponentInParent<bulletColour>().selectColor(colourItr);
        Debug.Log("color confirmed");
    }
    public void unconfirmColor()
    {
        GetComponentInParent<bulletColour>().unselectColor(colourItr);
        Debug.Log("color unselected");
    }
}
