using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotate : MonoBehaviour {


	public float rotX = 0.01f;

	// Update is called once per frame
	void Update () {
		
		float spin = rotX;

		transform.Rotate(spin, 0, 0);
	}
}
