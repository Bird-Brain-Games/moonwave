using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunGraphics : MonoBehaviour {


	public Renderer m_SunRenderer;
	public float m_timeScale = 0.01f;
	public float m_glowBrightness = 0.05f;

	// Update is called once per frame
	void Update () {
		


		m_SunRenderer.materials[1].SetVector("_Color", 
		new Vector4(Mathf.PingPong(Time.time * m_timeScale, m_glowBrightness) + 0.005f, 
			Mathf.PingPong(Time.time * m_timeScale, m_glowBrightness) + 0.005f, 
			Mathf.PingPong(Time.time * m_timeScale, m_glowBrightness) + 0.005f, 
			255));

	}
}
