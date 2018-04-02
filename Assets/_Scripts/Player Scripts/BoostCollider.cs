using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostCollider : MonoBehaviour
{

    // Use this for initialization
    PLayerBigHitState m_state;
    PLayerBigHitState m_tempState;
    PlayerStats m_stats;
    Transform m_parentTransform;
    BoxCollider m_BoxCollider;
    SpriteRenderer m_SpriteRenderer;
    Animator m_Animator;
    Renderer m_Rend;
    private Color color;

    private bool fixedUpdate;
    private Vector3 m_offset;
    private Quaternion m_rotation;
    public float setOffset;
    public bool fullyCharged;

    void Awake()
    {

        m_BoxCollider = GetComponent<BoxCollider>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_Rend = GetComponent<Renderer>();
        fixedUpdate = false;
        fullyCharged = false;
    }

    // public void SetCharged(bool isCharged)
    // {
    //     fullyCharged = isCharged;
    //     if (isCharged)
    //         m_Rend.material.color = color;
    //     else
    //         m_Rend.material.color = Color.white;
    // }

    public void SetCharged(int _isCharged)
    {
        // Workaround to use ints and bools
        bool isCharged = (_isCharged == 1);

        fullyCharged = isCharged;
        if (isCharged)
            m_Rend.material.color = color;
        else
            m_Rend.material.color = Color.white;
    }

    public void PlayerLink(PlayerStats p_player)
    {
        m_stats = p_player;
        m_state = p_player.GetComponent<PLayerBigHitState>();
        m_parentTransform = p_player.GetComponent<Transform>();

    }

    public void setColour(Color colour)
    {
        color = colour;
    }

    public void setCollider(Vector3 transform, Quaternion rotation, bool collision = true)
    {
        m_rotation = rotation;
        m_offset = transform;
        fixedUpdate = true;
        //hacky way to update the position and then make it visisble.
        FixedUpdate();
        m_BoxCollider.enabled = collision;
        m_SpriteRenderer.enabled = true;
        m_Animator.SetBool("Boosting", true);
    }

    private void FixedUpdate()
    {
        if (fixedUpdate)
        {
            transform.position = m_parentTransform.position;
            transform.rotation = Quaternion.identity;
            transform.Translate(m_offset * setOffset);
            transform.rotation = m_rotation;
            //transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
    }

    public void BoostEnded()
    {
        m_rotation = Quaternion.identity;
        m_offset = new Vector3();
        fixedUpdate = false;
        m_BoxCollider.enabled = false;
        m_SpriteRenderer.enabled = false;
        m_Animator.SetBool("Boosting", false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("boost collider");
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("boost collider with player");
            if (collider.transform != m_parentTransform)
            {
                //basically switch to the playerBigHitState;
                Debug.Log("boost collider with player other than themselves");
                //Set hitlastby
                if (collider.transform.GetComponent<PlayerStats>().Invincible == false)
                {
                    collider.gameObject.GetComponent<PlayerStats>().m_HitLastBy = m_stats;

                    if (collider.transform.GetComponent<PlayerStats>().GetShieldState() == false)
                    {
                        Debug.Log("boost collider without shield");


                        // SFX
                        FindObjectOfType<AudioManager>().Play("Crit");

                        //get the colliders BigHitState
                        m_tempState = collider.transform.GetComponentInParent<PLayerBigHitState>();

                        //calculate the force acting on the hit player
                        //var Force = collider.transform.position - m_stats.transform.position;
                        var Force = m_offset;
                        m_state.Direction = Force;
                        m_tempState.Direction = -Force;
                        Force.Normalize();

                        //Adds the force to the player we collided with
                        Force = (Force
                            * m_stats.m_boost.MaxForce
                            * m_stats.m_boost.boostCriticalHit);

                        collider.GetComponent<StateManager>().ChangeState(m_stats.PlayerBigHitState);
                        Physics.IgnoreCollision(m_stats.GetComponent<Collider>(), collider, true);
                        collider.GetComponent<PlayerStateManager>().SetCollider(m_stats.GetComponent<Collider>(), collider);

                        m_tempState.Force = Force;

                        m_state.GetComponent<StateManager>().ChangeState(m_stats.PlayerBigHitState);
                        m_state.isTarget = false;

                        m_BoxCollider.enabled = false;
                        m_SpriteRenderer.enabled = false;
                    }
                }
            }
            else
            {
                Debug.Log("Collision with ourselves");
                Debug.Log(collider.transform.GetInstanceID());
                Debug.Log(m_parentTransform.GetInstanceID());
            }
        }
    }
}