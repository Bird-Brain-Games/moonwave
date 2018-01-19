using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour {

	public GameObject pickup;
	public float m_xRange;
	public float m_yRange;
	public float m_delayMin;
	public float m_delayMax;

	public bool pickupsON = true;

	private bool m_timer;
	private float m_startTime;
	private float m_delay;
	private float m_time;


	// Use this for initialization
	void Start () {
		m_timer = false;
		m_delay = Random.Range(m_delayMin, m_delayMax);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(m_delay > m_time - m_startTime && !m_timer)
		{
			m_time = Time.time;
		}
		else if (m_delay < m_time - m_startTime && !m_timer)
		{
			m_timer = true;
		}
		
		
		if (m_timer){
			
			m_timer = false;
			m_startTime = Time.time;
			m_time = Time.time;
			m_delay = Random.Range(m_delayMin, m_delayMax);

			if (pickupsON){

				Instantiate(
				pickup, 
				new Vector3(Random.Range(-m_xRange,m_xRange), Random.Range(-m_yRange,m_yRange), 0),
				Quaternion.identity
				);
			}
		}

	}
}
