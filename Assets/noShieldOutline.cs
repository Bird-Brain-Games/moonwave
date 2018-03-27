using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noShieldOutline : MonoBehaviour {

	public Material m_OutlineMaterial;
	
	private Material[] m_mats;

	private SkinnedMeshRenderer m_renderer;

	public Shield m_shield;

	// Use this for initialization
	void Start () {
		
		m_renderer = GetComponent<SkinnedMeshRenderer>();
		

		m_mats = m_renderer.materials;

		m_mats[1] = null;

		m_renderer.materials = m_mats;
	}
	
	// Update is called once per frame
	void Update () {

		
			if (m_shield.m_shieldHealth <= 0)
			{
				
			}
			else
			{

			}



	}
}
