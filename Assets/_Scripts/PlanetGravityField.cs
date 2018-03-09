using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravityField : MonoBehaviour {

    public float m_GravityStrength;
    static float m_GravitationalConstant = 15;

    float m_OrbitDistance;
    Collider m_GravityTrigger;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float CalculateGravitationalForce(float objectMass, float distSquared)
    {
        return m_GravitationalConstant * ((m_GravityStrength * objectMass) / distSquared);
    }
}
