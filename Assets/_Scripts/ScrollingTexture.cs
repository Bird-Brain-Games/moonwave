using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour {


	

	public float scrollX = 0.2f;
	public float scrollY = 0.2f;

	// Update is called once per frame
	void Update () {
		float offsetX = Mathf.PingPong(Time.time * 0.1f, Mathf.PingPong(Time.time * 0.2f, 0.2f));
		float offsetY = Mathf.PingPong(Time.time * 0.2f, Mathf.PingPong(Time.time * 0.2f, 0.1f));

		GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
	}
}
