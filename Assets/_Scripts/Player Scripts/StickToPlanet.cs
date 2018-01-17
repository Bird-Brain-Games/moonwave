using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToPlanet : MonoBehaviour
{
    /// <summary>
    /// Adapted from
    /// https://github.com/nlra/Galaxy/blob/master/Assets/Game/Scripts/StickToPlanet.cs
    /// http://www.asteroidbase.com/devlog/7-learning-how-to-walk/
    /// https://gamedev.stackexchange.com/questions/89693/how-could-i-constrain-player-movement-to-the-surface-of-a-3d-object-using-unity
    /// https://mikeloscocco.wordpress.com/2015/10/13/mario-galaxy-physics-in-unity/
    /// </summary>

    Rigidbody m_RigidBody;
    Collider m_Collider;
    PlayerStats m_PlayerStats;

    Vector3 m_CurrentGravityDirection;
    PlanetGravityField m_CurrentPlanet;
    List<Collider> m_PlanetsAffecting;
    Quaternion m_PreviousRotation;

    float m_PreviousGravityFieldStrength;

    float linkDistance = 100.0f;
    bool m_CollidedWithPlanet;
    float m_DistanceToGround;

    [Tooltip("How large the turning angle of the player can be")]
    public float m_TurningSpeed = 5.0f;

    [Tooltip("If the equation used for resolving gravity between\n multiple planets is constant or Newtonian")]
    public bool constantGravity = true;

    // Logging: l_ is used to indicate a variable used in logging
    public float l_groundTime;
    public float l_hangTime;

    // Use this for initialization
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_PlayerStats = GetComponent<PlayerStats>();
        m_CurrentPlanet = null;
        m_PlanetsAffecting = new List<Collider>();
        m_DistanceToGround = m_Collider.bounds.extents.y;

        l_hangTime = 0.0f;
        l_groundTime = 0.0f;
    }

    Vector3 GetGravity(RaycastHit hit1, float gravityForce)
    {
        // Rotate the player
        Vector3 rotationDirection = hit1.normal;
        if (Vector3.Angle(transform.up, hit1.normal) > 5.0f)
        {
            // Probably don't need the if check, the turning speed does it for us
            rotationDirection = Vector3.RotateTowards(transform.up, hit1.normal, m_TurningSpeed * Time.fixedDeltaTime, 1.0f);
        }
        m_PreviousRotation = Quaternion.LookRotation(transform.forward, rotationDirection);
        m_RigidBody.MoveRotation(m_PreviousRotation);

        // Apply Gravity
        float distSquared = (hit1.transform.position - transform.position).sqrMagnitude;
        //gravityForce = m_CurrentPlanet.CalculateGravitationalForce(
        //m_RigidBody.mass, distSquared);
        //m_RigidBody.AddForce(gravityForce * -rotationDirection);

        return (gravityForce * -rotationDirection);
    }

    public Vector3 DriftingUpdate()
    {
        l_hangTime += Time.deltaTime;
        Vector3 result = Vector3.zero;

        if (PlanetInRange() && OnlyOnePlanetInRange())     // Only one planet
        {
            //Debug.Log("One planet");
            // Try to hit directly below the player
            RaycastHit hit1;
            bool hitPlanet = Physics.Raycast(m_RigidBody.position, -transform.up,
                out hit1, linkDistance, LayerMask.GetMask("Planet"), QueryTriggerInteraction.Ignore);

            // Apply gravity if it finds a planet directly below the player
            if (hitPlanet)
            {
                result = GetGravity(hit1, m_CurrentPlanet.m_GravityStrength);
                Debug.DrawLine(m_RigidBody.position, hit1.point);
            }

            // Shoot ray directed towards the center of the planet
            else if (Physics.Raycast(m_RigidBody.position, (m_CurrentPlanet.transform.position - m_RigidBody.position).normalized,
                out hit1, linkDistance, LayerMask.GetMask("Planet"), QueryTriggerInteraction.Ignore))
            {
                result = GetGravity(hit1, m_CurrentPlanet.m_GravityStrength);
                Debug.DrawLine(m_RigidBody.position, hit1.point);
                Debug.Log("Backup Gravity");
            }

            return result;
        }
        else if (PlanetInRange())   // Multiple planets
        {
            //RaycastHit hit;

            Debug.Log("Multple Planets in range");
            Vector3 gravityForce = Vector3.zero;

            foreach (Collider planet in m_PlanetsAffecting)
            {
                Vector3 diff = (planet.transform.position - m_RigidBody.position);
                float distance = diff.sqrMagnitude;
                Vector3 direction = diff.normalized;
                Debug.DrawLine(m_RigidBody.position, m_RigidBody.position + direction * 2.0f);

                if (constantGravity)
                {
                    gravityForce += direction * planet.GetComponent<PlanetGravityField>().m_GravityStrength;  // Constant gravity
                }
                else
                {
                    gravityForce += direction * planet.GetComponent<PlanetGravityField>().
                        CalculateGravitationalForce(m_RigidBody.mass, distance);    // Newton Gravity
                }


                //if (Physics.Raycast(transform.position, direction, out hit,
                //    linkDistance, LayerMask.GetMask("Planet"), QueryTriggerInteraction.Ignore))
                //{

                //}
            }

            // Rotate player towards the gravity
            //Vector3 rotationDirection = Vector3.RotateTowards(transform.up, gravityForce.normalized, 100, 1.0f);
            //m_PreviousRotation = Quaternion.LookRotation(transform.forward, -gravityForce.normalized);
            //transform.rotation = m_PreviousRotation;

            // Apply the gravity
            //m_RigidBody.AddForce(gravityForce);
            Debug.DrawLine(m_RigidBody.position, m_RigidBody.position + gravityForce.normalized * 2.0f, Color.green);
            return gravityForce;
        }

        //return m_CollidedWithPlanet;
        return Vector3.zero;
    }

    public void GroundedUpdate()
    {
        l_groundTime += Time.deltaTime;

        if (PlanetInRange())    // Grounded on the planet
        {
            RaycastHit hit1;
            bool hitPlanet = Physics.Raycast(m_RigidBody.position, -transform.up,
                out hit1, linkDistance, LayerMask.GetMask("Planet"), QueryTriggerInteraction.Ignore);

            // Stick to planet if it finds a planet directly below the player
            if (hitPlanet)
            {
                if (hit1.distance > m_DistanceToGround)
                {
                    m_RigidBody.MovePosition(m_RigidBody.position + (-transform.up * (hit1.distance - m_DistanceToGround)));
                }
                m_RigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, hit1.normal));
                m_RigidBody.AddForce(m_CurrentPlanet.m_GravityStrength * -transform.up);
            }
            // Shoot ray directed towards the center of the planet
            else if (Physics.Raycast(m_RigidBody.position, (m_CurrentPlanet.transform.position - m_RigidBody.position).normalized,
                out hit1, linkDistance, LayerMask.GetMask("Planet"), QueryTriggerInteraction.Ignore))
            {
                if (hit1.distance > m_DistanceToGround)
                {
                    m_RigidBody.MovePosition(m_RigidBody.position + (-transform.up * (hit1.distance - m_DistanceToGround)));
                }
                m_RigidBody.MoveRotation(Quaternion.LookRotation(transform.forward, hit1.normal));
                m_RigidBody.AddForce(m_CurrentPlanet.m_GravityStrength * -transform.up);
                Debug.Log("Backup Gravity");
            }
        }
    }

    void FixedUpdate()
    {
        // Reset the collision check
        //m_CollidedWithPlanet = false;
    }

    void OnTriggerEnter(Collider other)
    {

        if (tag == "Player")
        {
            if (other.tag == "Planet")
            {
                if (!m_PlanetsAffecting.Contains(other))
                {
                    m_PlanetsAffecting.Add(other);
                    Debug.Log("Entering " + other.name);

                    if (OnlyOnePlanetInRange())
                        m_CurrentPlanet = other.gameObject.GetComponent<PlanetGravityField>();

                    RotateTowardsCurrentPlanet();
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Planet")
        {
            if (m_PlanetsAffecting.Contains(other))
            {
                Debug.Log("Leaving " + other.name);
                m_PlanetsAffecting.Remove(other);
            }
        }

        //Debug.Log(m_PlanetsAffecting.Count);
        if (!PlanetInRange())
        {
            m_CurrentPlanet = null;
        }
        else if (OnlyOnePlanetInRange())
        {
            m_CurrentPlanet = m_PlanetsAffecting[0].GetComponent<PlanetGravityField>();
            Debug.Log("Current Planet: " + m_CurrentPlanet.name);
            RotateTowardsCurrentPlanet();
        }
    }

    bool PlanetInRange()
    {
        return (m_PlanetsAffecting.Count > 0);
    }

    bool OnlyOnePlanetInRange()
    {
        return (m_PlanetsAffecting.Count == 1);
    }

    public bool FindIfGrounded()
    {
        // Inspiration from http://answers.unity3d.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
        RaycastHit hit;
        if (!Physics.Raycast(m_RigidBody.position, -transform.up, out hit, m_DistanceToGround + 0.1f,
            LayerMask.GetMask("Planet"), QueryTriggerInteraction.Ignore))
            return false;

        return (hit.transform.tag == "Planet");

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            //m_CollidedWithPlanet = true;

            m_CurrentPlanet = collision.gameObject.GetComponent<PlanetGravityField>();
            //Debug.Log("Player Collided with " + collision.gameObject.name);

            // Rotate player towards planet
            RotateTowardsCurrentPlanet();

        }

        else if (collision.gameObject.CompareTag("Hazard"))
        {
            // SFX
            FindObjectOfType<AudioManager>().Play("Lava Death");
            Debug.Log("hazard");
            GetComponent<KnockOut>().ResetPlayer();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            //m_CollidedWithPlanet = true;
        }
    }

    float GetGravityStrength(RaycastHit hit)
    {
        if (hit.collider.tag == "Planet")
        {
            return hit.collider.GetComponent<PlanetGravityField>().m_GravityStrength;
        }
        return -1.0f;
    }

    void RotateTowardsCurrentPlanet()
    {
        // Want to smoothly rotate towards new planet, this will do for now
        Vector3 planetDir = (m_RigidBody.position - m_CurrentPlanet.transform.position).normalized;
        Quaternion lookAtPlanet = Quaternion.LookRotation(transform.forward, planetDir);
        transform.rotation = Quaternion.RotateTowards(m_RigidBody.rotation, lookAtPlanet, 180.0f);
    }

    public Vector3 GetDirectionOfCurrentPlanet()
    {
        if (m_CurrentPlanet)
            return (m_RigidBody.position - m_CurrentPlanet.transform.position).normalized;
        return Vector3.zero;
    }
}
