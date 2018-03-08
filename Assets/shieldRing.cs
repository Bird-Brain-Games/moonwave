using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldRing : MonoBehaviour {

	
	public float m_shieldThreshold = 0;
	public float m_colourLerpTime = 1;
	private SpriteRenderer spriteRenderer;
	
	private Shield parentShield;
	private Rigidbody playerRigidBody;
	private float shieldHP;
	private Color playerColor;

	
	// Use this for initialization
	void Start () {

		
		
		spriteRenderer = this.transform.GetComponent<SpriteRenderer>();
		playerRigidBody = this.transform.parent.parent.GetComponent<Rigidbody>();
		parentShield = this.transform.parent.GetComponent<Shield>();

		playerColor = this.transform.parent.parent.GetComponent<PlayerStats>().colour;
		playerColor = playerColor * 2.5f; // Brightens the colour
		spriteRenderer.color = playerColor;
		
		this.transform.localScale = new Vector3(0.8f, 0.2f, 1);

	}
	
	// Update is called once per frame
	void Update () {
		
		//spriteRenderer.color = Color.Lerp(Color.white, playerColor, Mathf.PingPong(Time.time, m_colourLerpTime));
		
		this.transform.Rotate(0,0, 0.5f);

		shieldHP = parentShield.m_shieldHealth;

		if(shieldHP > m_shieldThreshold)
		{
			spriteRenderer.enabled = true;
		}
		else{
			spriteRenderer.enabled = false;
		}
	}
}
