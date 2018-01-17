using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOnPlanet : MonoBehaviour
{
    Rigidbody m_RigidBody;
    StickToPlanet m_Gravity;
    Vector2 m_move;
    public int m_MovementType = 1;
    float dotProduct;
    public float friction;

    Vector3 m_JoyStick;
    Vector3 m_PlanetToPlayer;
    Controls controls;
    PlayerStats m_PlayerStats;

    // Use this for initialization
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Gravity = GetComponent<StickToPlanet>();
        controls = GetComponent<Controls>();
        m_PlayerStats = GetComponent<PlayerStats>();
    }


    // Update called once per physics update
    public void MoveOnPlanet()
    {
        m_move = controls.GetMove();
        if (m_MovementType == 0)
        {
                m_RigidBody.velocity = transform.right * m_move.x * m_PlayerStats.walkMoveForce;
                //m_RigidBody.AddForce(transform.right * m_hAxis * controls.walkMoveForce, ForceMode.VelocityChange);
        }
        else if (m_MovementType == 1)
        {
            //create joystick vector and normalize it
            m_JoyStick = new Vector3(m_move.x, m_move.y, 0.0f);
            m_JoyStick.Normalize();

            //dot the joystick and player up vector

            m_PlanetToPlayer = -m_Gravity.GetDirectionOfCurrentPlanet();//new Vector3 (transform.position - )
            dotProduct = Vector3.Dot(m_JoyStick, m_PlanetToPlayer);
            //Debug.Log("Dot product " + Vector3.Dot(m_JoyStick, transform.up));
            //Debug.Log("H axis: " + m_move.x);
            //Debug.Log("V axis: " + m_move.y);
            //Debug.Log("dotProduct: " + dotProduct);
            if (dotProduct > -0.95f || dotProduct < -1.05f)
            {
                if (m_JoyStick.y * m_PlanetToPlayer.x > m_JoyStick.x * m_PlanetToPlayer.y)
                {

                    //m_RigidBody.AddForce(transform.right * controls.walkMoveForce * -1, ForceMode.VelocityChange);
                    //m_RigidBody.velocity = Vector3.ClampMagnitude(m_RigidBody.velocity, m_MaxWalkSpeed);


                    //Debug.Log("Move counter clockwise");
                    m_RigidBody.velocity = transform.right * m_PlayerStats.walkMoveForce * 1.0f;
                    if (transform.localScale.x == 1f)
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    //Debug.Log("Move clockwise");
                    m_RigidBody.velocity = transform.right * m_PlayerStats.walkMoveForce * -1.0f;
                    if (transform.localScale.x == -1f)
                        transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            else
            {
                // Apply friction
                m_RigidBody.velocity += -m_RigidBody.velocity * friction;
                if (m_RigidBody.velocity.magnitude < 50f)
                {
                    m_RigidBody.velocity.Set(0, 0, 0);
                }
            }

        }
        //IF the player isnt moving apply friction
        else if (m_move.x == 0.0f && m_RigidBody.velocity != Vector3.zero)
        {
            m_RigidBody.velocity += -m_RigidBody.velocity * friction;
            if (m_RigidBody.velocity.magnitude < 0.1f)
            {
                m_RigidBody.velocity.Set(0, 0, 0);
            }
        }
        

    
        // Debug.Log(m_RigidBody.velocity);
        // Debug.Log(m_hAxis);
    }
}
